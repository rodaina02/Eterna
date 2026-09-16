namespace Eterna.Web.ViewModels;

public sealed class PageCtaViewModel
{
    public string? Index { get; init; }
    public string? Eyebrow { get; init; }
    public string Title { get; init; } = string.Empty;
    public IReadOnlyList<string> TitleLines { get; init; } = [];
    public string LinkText { get; init; } = "Start a project";
    public string Href { get; init; } = "/contact";
    public string HeadingId { get; init; } = "page-cta-heading";
}
