using Eterna.Application.Abstractions;
using Eterna.Application.Contacts;
using Eterna.Application.DTOs;
using Eterna.Application.Options;
using Eterna.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eterna.Application.Tests;

public sealed class ContactSubmissionServiceTests
{
    [Fact]
    public async Task Validation_failure_does_not_persist_or_notify()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService();
        var service = CreateService(repository, email);

        var request = ContactFormValidatorTests.Valid();
        request.Name = "";

        var result = await service.SubmitAsync(request, Context());

        Assert.Equal(ContactSubmitOutcome.ValidationFailed, result.Outcome);
        Assert.Empty(repository.Items);
        Assert.Empty(email.Notifications);
        Assert.Empty(email.Confirmations);
    }

    [Fact]
    public async Task Honeypot_is_accepted_without_sending_email()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService();
        var service = CreateService(repository, email);

        var request = ContactFormValidatorTests.Valid();
        request.Website = "https://spam.example";

        var result = await service.SubmitAsync(request, Context());

        Assert.Equal(ContactSubmitOutcome.Accepted, result.Outcome);
        Assert.Single(repository.Items);
        Assert.True(repository.Items[0].HoneypotHit);
        Assert.Empty(email.Notifications);
        Assert.Empty(email.Confirmations);
    }

    [Fact]
    public async Task Valid_submission_persists_hashes_ip_and_sends_emails()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService();
        var service = CreateService(repository, email);

        var result = await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        Assert.Equal(ContactSubmitOutcome.Accepted, result.Outcome);
        var stored = Assert.Single(repository.Items);
        Assert.False(stored.HoneypotHit);
        Assert.True(stored.EmailSent);
        Assert.Equal(64, stored.IpHash!.Length);
        Assert.NotEqual("203.0.113.10", stored.IpHash);
        Assert.Single(email.Notifications);
        Assert.Single(email.Confirmations);
        Assert.Equal("amira@example.com", email.Notifications[0].Email);
    }

    [Fact]
    public async Task Persistence_failure_returns_persistence_failed()
    {
        var repository = new FakeContactRepository { AddException = new InvalidOperationException("db") };
        var email = new FakeEmailService();
        var service = CreateService(repository, email);

        var result = await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        Assert.Equal(ContactSubmitOutcome.PersistenceFailed, result.Outcome);
        Assert.Empty(email.Notifications);
    }

    [Fact]
    public async Task Notification_failure_does_not_report_success()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService { NotificationSucceeds = false };
        var service = CreateService(repository, email);

        var result = await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        Assert.Equal(ContactSubmitOutcome.NotificationFailed, result.Outcome);
        var stored = Assert.Single(repository.Items);
        Assert.False(stored.EmailSent);
        Assert.Empty(email.Confirmations);
    }

    [Fact]
    public async Task Confirmation_failure_still_accepts_after_notification()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService { ConfirmationSucceeds = false };
        var service = CreateService(repository, email);

        var result = await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        Assert.Equal(ContactSubmitOutcome.Accepted, result.Outcome);
        Assert.True(repository.Items[0].EmailSent);
    }

    [Fact]
    public async Task Confirmation_is_skipped_when_disabled()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService();
        var service = CreateService(repository, email, sendConfirmation: false);

        var result = await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        Assert.Equal(ContactSubmitOutcome.Accepted, result.Outcome);
        Assert.Single(email.Notifications);
        Assert.Empty(email.Confirmations);
    }

    [Fact]
    public async Task Missing_hash_secret_does_not_store_plaintext_ip()
    {
        var repository = new FakeContactRepository();
        var email = new FakeEmailService();
        var service = CreateService(repository, email, ipHashSecret: "");

        await service.SubmitAsync(ContactFormValidatorTests.Valid(), Context());

        var stored = Assert.Single(repository.Items);
        Assert.Null(stored.IpHash);
    }

    private static ContactSubmissionService CreateService(
        FakeContactRepository repository,
        FakeEmailService email,
        string ipHashSecret = "unit-test-secret",
        bool sendConfirmation = true)
    {
        return new ContactSubmissionService(
            new ContactFormValidator(),
            repository,
            email,
            new FixedClock(),
            Microsoft.Extensions.Options.Options.Create(new ContactOptions { IpHashSecret = ipHashSecret }),
            Microsoft.Extensions.Options.Options.Create(new EmailOptions { SendVisitorConfirmation = sendConfirmation }),
            NullLogger<ContactSubmissionService>.Instance);
    }

    private static ContactSubmitContext Context() => new("203.0.113.10", "EternaTests/1.0");

    private sealed class FixedClock : IClock
    {
        public DateTimeOffset UtcNow { get; } = new(2026, 9, 17, 8, 0, 0, TimeSpan.Zero);
    }

    private sealed class FakeContactRepository : IContactSubmissionRepository
    {
        public List<ContactSubmission> Items { get; } = [];
        public Exception? AddException { get; set; }

        public Task AddAsync(ContactSubmission submission, CancellationToken cancellationToken = default)
        {
            if (AddException is not null)
            {
                throw AddException;
            }

            Items.Add(submission);
            return Task.CompletedTask;
        }

        public Task UpdateEmailDispatchAsync(
            Guid id,
            bool emailSent,
            DateTimeOffset? emailSentAt,
            CancellationToken cancellationToken = default)
        {
            var item = Items.Single(submission => submission.Id == id);
            item.EmailSent = emailSent;
            item.EmailSentAt = emailSentAt;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeEmailService : IEmailService
    {
        public bool NotificationSucceeds { get; set; } = true;
        public bool ConfirmationSucceeds { get; set; } = true;
        public List<ContactNotificationDto> Notifications { get; } = [];
        public List<ContactConfirmationDto> Confirmations { get; } = [];

        public Task<EmailSendResult> SendNotificationAsync(
            ContactNotificationDto notification,
            CancellationToken cancellationToken = default)
        {
            Notifications.Add(notification);
            return Task.FromResult(new EmailSendResult(NotificationSucceeds, NotificationSucceeds ? null : "failed"));
        }

        public Task<EmailSendResult> SendConfirmationAsync(
            ContactConfirmationDto confirmation,
            CancellationToken cancellationToken = default)
        {
            Confirmations.Add(confirmation);
            return Task.FromResult(new EmailSendResult(ConfirmationSucceeds, ConfirmationSucceeds ? null : "failed"));
        }
    }
}
