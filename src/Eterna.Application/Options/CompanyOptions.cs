namespace Eterna.Application.Options;

public sealed class CompanyOptions
{
    public const string SectionName = "Company";

    public string Name { get; set; } = "Eterna";
    public string Slogan { get; set; } = "Building Intelligent Legacies";
    public string Statement { get; set; } =
        "Software and AI solutions dedicated to building durable digital products and intelligent systems.";
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SocialHandle { get; set; } = string.Empty;
    public string SocialUrl { get; set; } = string.Empty;
}
