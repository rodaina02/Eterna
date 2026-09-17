namespace Eterna.Application.Contacts;

public enum ContactSubmitOutcome
{
    Accepted = 0,
    ValidationFailed = 1,
    PersistenceFailed = 2,
    NotificationFailed = 3
}

public sealed class ContactSubmitResult
{
    public ContactSubmitOutcome Outcome { get; private init; }

    public IReadOnlyDictionary<string, string[]> Errors { get; private init; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);

    public Guid? SubmissionId { get; private init; }

    public static ContactSubmitResult Accepted(Guid submissionId) => new()
    {
        Outcome = ContactSubmitOutcome.Accepted,
        SubmissionId = submissionId
    };

    public static ContactSubmitResult ValidationFailed(IDictionary<string, string[]> errors) => new()
    {
        Outcome = ContactSubmitOutcome.ValidationFailed,
        Errors = new Dictionary<string, string[]>(errors, StringComparer.Ordinal)
    };

    public static ContactSubmitResult PersistenceFailed() => new()
    {
        Outcome = ContactSubmitOutcome.PersistenceFailed
    };

    public static ContactSubmitResult NotificationFailed(Guid submissionId) => new()
    {
        Outcome = ContactSubmitOutcome.NotificationFailed,
        SubmissionId = submissionId
    };
}
