namespace Eterna.Application.Contacts;

public sealed class ContactFormRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Service { get; set; } = string.Empty;
    public string? Budget { get; set; }
    public string Message { get; set; } = string.Empty;

    /// <summary>Honeypot. Must remain empty.</summary>
    public string? Website { get; set; }
}
