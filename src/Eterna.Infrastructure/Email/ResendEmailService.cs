using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Eterna.Application.Abstractions;
using Eterna.Application.DTOs;
using Eterna.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eterna.Infrastructure.Email;

public sealed class ResendEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly EmailOptions _options;
    private readonly ILogger<ResendEmailService> _logger;

    public ResendEmailService(
        HttpClient httpClient,
        IOptions<EmailOptions> options,
        ILogger<ResendEmailService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public Task<EmailSendResult> SendNotificationAsync(
        ContactNotificationDto notification,
        CancellationToken cancellationToken = default)
    {
        if (!IsConfigured())
        {
            _logger.LogWarning("Resend is not configured. Notification email was not sent.");
            return Task.FromResult(new EmailSendResult(false, "Resend is not configured."));
        }

        var subject = "New Eterna Project Inquiry";
        var html = $"""
            <p>A new project inquiry was submitted on the Eterna website.</p>
            <p><strong>Name:</strong> {Encode(notification.Name)}</p>
            <p><strong>Company:</strong> {Encode(notification.Company)}</p>
            <p><strong>Email:</strong> {Encode(notification.Email)}</p>
            <p><strong>Phone:</strong> {Encode(notification.Phone)}</p>
            <p><strong>Service:</strong> {Encode(notification.Service)}</p>
            <p><strong>Budget:</strong> {Encode(notification.Budget)}</p>
            <p><strong>Message:</strong></p>
            <p>{FormatMessage(notification.Message)}</p>
            <p><strong>Submitted:</strong> {notification.SubmittedAt.UtcDateTime:yyyy-MM-dd HH:mm} UTC</p>
            """;

        return SendAsync(_options.NotificationEmail, subject, html, cancellationToken);
    }

    public Task<EmailSendResult> SendConfirmationAsync(
        ContactConfirmationDto confirmation,
        CancellationToken cancellationToken = default)
    {
        if (!_options.SendVisitorConfirmation)
        {
            return Task.FromResult(new EmailSendResult(true));
        }

        if (!IsConfigured())
        {
            _logger.LogWarning("Resend is not configured. Confirmation email was not sent.");
            return Task.FromResult(new EmailSendResult(false, "Resend is not configured."));
        }

        var subject = "Your project details were received — Eterna";
        var html = $"""
            <p>Hello {Encode(confirmation.Name)},</p>
            <p>Thank you for reaching out to Eterna. Your project details have been received.</p>
            <p>Building Intelligent Legacies.</p>
            """;

        return SendAsync(confirmation.Email, subject, html, cancellationToken);
    }

    private bool IsConfigured()
    {
        return !string.IsNullOrWhiteSpace(_options.ApiKey)
            && !string.IsNullOrWhiteSpace(_options.FromEmail)
            && !string.IsNullOrWhiteSpace(_options.NotificationEmail);
    }

    private async Task<EmailSendResult> SendAsync(
        string to,
        string subject,
        string html,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new ResendEmailRequest
            {
                From = $"{SanitizeHeader(_options.FromName)} <{SanitizeHeader(_options.FromEmail)}>",
                To = [SanitizeHeader(to)],
                Subject = SanitizeHeader(subject),
                Html = html
            };

            using var response = await _httpClient.PostAsJsonAsync("emails", payload, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new EmailSendResult(true);
            }

            _logger.LogError(
                "Resend rejected an email with status {StatusCode}.",
                (int)response.StatusCode);

            return new EmailSendResult(false, "Resend rejected the message.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Resend request failed.");
            return new EmailSendResult(false, "Resend request failed.");
        }
    }

    private static string Encode(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? "—"
            : System.Net.WebUtility.HtmlEncode(value);
    }

    private static string FormatMessage(string? value)
    {
        return Encode(value)
            .Replace("\r\n", "<br />", StringComparison.Ordinal)
            .Replace("\n", "<br />", StringComparison.Ordinal)
            .Replace("\r", "<br />", StringComparison.Ordinal);
    }

    private static string SanitizeHeader(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
    }

    private sealed class ResendEmailRequest
    {
        [JsonPropertyName("from")]
        public string From { get; set; } = string.Empty;

        [JsonPropertyName("to")]
        public string[] To { get; set; } = [];

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("html")]
        public string Html { get; set; } = string.Empty;
    }
}
