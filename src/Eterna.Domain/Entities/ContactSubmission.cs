using Eterna.Domain.Enums;

namespace Eterna.Domain.Entities;

public sealed class ContactSubmission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Service { get; set; } = string.Empty;
    public string? Budget { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset SubmittedAt { get; set; }
    public bool EmailSent { get; set; }
    public DateTimeOffset? EmailSentAt { get; set; }
    public ContactStatus Status { get; set; } = ContactStatus.New;
    public bool HoneypotHit { get; set; }
    public string? IpHash { get; set; }
    public string? UserAgent { get; set; }
}
