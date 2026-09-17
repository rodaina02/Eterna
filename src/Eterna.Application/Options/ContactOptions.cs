namespace Eterna.Application.Options;

public sealed class ContactOptions
{
    public const string SectionName = "Contact";

    /// <summary>
    /// Server-side secret used to hash client IP addresses before persistence.
    /// Set via user secrets or environment variable <c>Contact__IpHashSecret</c>.
    /// </summary>
    public string IpHashSecret { get; set; } = string.Empty;
}
