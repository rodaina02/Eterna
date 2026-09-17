using Eterna.Application.Abstractions;
using Eterna.Application.DTOs;
using Eterna.Application.Options;
using Eterna.Domain.Entities;
using Eterna.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eterna.Application.Contacts;

public sealed class ContactSubmissionService : IContactSubmissionService
{
    private readonly IValidator<ContactFormRequest> _validator;
    private readonly IContactSubmissionRepository _submissions;
    private readonly IEmailService _email;
    private readonly IClock _clock;
    private readonly ContactOptions _contactOptions;
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<ContactSubmissionService> _logger;

    public ContactSubmissionService(
        IValidator<ContactFormRequest> validator,
        IContactSubmissionRepository submissions,
        IEmailService email,
        IClock clock,
        IOptions<ContactOptions> contactOptions,
        IOptions<EmailOptions> emailOptions,
        ILogger<ContactSubmissionService> logger)
    {
        _validator = validator;
        _submissions = submissions;
        _email = email;
        _clock = clock;
        _contactOptions = contactOptions.Value;
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }

    public async Task<ContactSubmitResult> SubmitAsync(
        ContactFormRequest request,
        ContactSubmitContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        var honeypotHit = !string.IsNullOrWhiteSpace(request.Website);
        if (honeypotHit)
        {
            return await HandleHoneypotAsync(request, context, cancellationToken);
        }

        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ContactSubmitResult.ValidationFailed(validation.ToDictionary());
        }

        var submittedAt = _clock.UtcNow;
        var submission = CreateSubmission(request, context, submittedAt, honeypotHit: false);

        try
        {
            await _submissions.AddAsync(submission, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Contact submission persistence failed.");
            return ContactSubmitResult.PersistenceFailed();
        }

        var notification = await _email.SendNotificationAsync(
            new ContactNotificationDto(
                submission.Name,
                submission.Company,
                submission.Email,
                submission.Phone,
                submission.Service,
                submission.Budget,
                submission.Message,
                submission.SubmittedAt),
            cancellationToken);

        if (!notification.Succeeded)
        {
            _logger.LogError(
                "Contact notification email failed for submission {SubmissionId}.",
                submission.Id);

            try
            {
                await _submissions.UpdateEmailDispatchAsync(
                    submission.Id,
                    emailSent: false,
                    emailSentAt: null,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to record notification failure for submission {SubmissionId}.",
                    submission.Id);
            }

            return ContactSubmitResult.NotificationFailed(submission.Id);
        }

        var sentAt = _clock.UtcNow;
        try
        {
            await _submissions.UpdateEmailDispatchAsync(
                submission.Id,
                emailSent: true,
                emailSentAt: sentAt,
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Notification was sent but dispatch flags could not be updated for submission {SubmissionId}.",
                submission.Id);
        }

        if (_emailOptions.SendVisitorConfirmation)
        {
            var confirmation = await _email.SendConfirmationAsync(
                new ContactConfirmationDto(submission.Name, submission.Email),
                cancellationToken);

            if (!confirmation.Succeeded)
            {
                _logger.LogWarning(
                    "Visitor confirmation email failed for submission {SubmissionId}.",
                    submission.Id);
            }
        }

        _logger.LogInformation("Contact submission {SubmissionId} was accepted.", submission.Id);
        return ContactSubmitResult.Accepted(submission.Id);
    }

    private async Task<ContactSubmitResult> HandleHoneypotAsync(
        ContactFormRequest request,
        ContactSubmitContext context,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsValid)
        {
            try
            {
                var submission = CreateSubmission(request, context, _clock.UtcNow, honeypotHit: true);
                await _submissions.AddAsync(submission, cancellationToken);
                _logger.LogInformation("Contact submission {SubmissionId} was stored.", submission.Id);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Contact submission persistence failed.");
            }
        }

        return ContactSubmitResult.Accepted(Guid.Empty);
    }

    private ContactSubmission CreateSubmission(
        ContactFormRequest request,
        ContactSubmitContext context,
        DateTimeOffset submittedAt,
        bool honeypotHit)
    {
        if (!string.IsNullOrWhiteSpace(context.RemoteIpAddress)
            && string.IsNullOrWhiteSpace(_contactOptions.IpHashSecret))
        {
            _logger.LogWarning(
                "Contact:IpHashSecret is not configured. Client address will not be stored.");
        }

        return new ContactSubmission
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Company = NormalizeOptional(request.Company, 160),
            Email = request.Email.Trim(),
            Phone = NormalizeOptional(request.Phone, 40),
            Service = request.Service.Trim(),
            Budget = NormalizeOptional(request.Budget, 40),
            Message = request.Message.Trim(),
            SubmittedAt = submittedAt,
            EmailSent = false,
            EmailSentAt = null,
            Status = ContactStatus.New,
            HoneypotHit = honeypotHit,
            IpHash = IpHasher.Hash(context.RemoteIpAddress, _contactOptions.IpHashSecret),
            UserAgent = SanitizeUserAgent(context.UserAgent)
        };
    }

    private static string? NormalizeOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }

    private static string? SanitizeUserAgent(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return null;
        }

        var cleaned = new string(userAgent.Trim().Where(character => !char.IsControl(character)).ToArray());
        if (cleaned.Length == 0)
        {
            return null;
        }

        return cleaned.Length <= 256 ? cleaned : cleaned[..256];
    }
}
