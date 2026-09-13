namespace Eterna.Application.Options;

public sealed class SiteOptions
{
    public const string SectionName = "Site";

    public string CanonicalBaseUrl { get; set; } = string.Empty;
    public string DefaultTitle { get; set; } = "Eterna — Building Intelligent Legacies";
    public string DefaultDescription { get; set; } =
        "Eterna is a software and AI solutions company dedicated to building durable digital products and intelligent systems for modern enterprises.";
    public string OgImagePath { get; set; } = "/images/brand/wordmark-lime-on-dark.png";
}
