namespace Eterna.Application.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Resend";

    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Eterna";
    public string NotificationEmail { get; set; } = string.Empty;
    public bool SendVisitorConfirmation { get; set; } = true;
}
