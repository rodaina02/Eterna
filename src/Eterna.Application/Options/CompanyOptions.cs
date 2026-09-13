namespace Eterna.Application.Options;

public sealed class CompanyOptions
{
    public const string SectionName = "Company";

    public string Name { get; set; } = "Eterna";
    public string Slogan { get; set; } = "Building Intelligent Legacies";
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SocialHandle { get; set; } = string.Empty;
    public string SocialUrl { get; set; } = string.Empty;
}
