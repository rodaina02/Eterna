namespace Eterna.Web.ViewModels;

public sealed class PageIntroViewModel
{
    public string Index { get; init; } = "01";
    public string Eyebrow { get; init; } = string.Empty;
    public string? Kicker { get; init; }
    public string Title { get; init; } = string.Empty;
    public IReadOnlyList<string> TitleLines { get; init; } = [];
    public string? Lede { get; init; }
    public IReadOnlyList<string> Meta { get; init; } = [];
    public string MetaLabel { get; init; } = "Practice areas";
    public string HeadingId { get; init; } = "page-heading";
}
