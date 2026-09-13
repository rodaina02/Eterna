namespace Eterna.Web.ViewModels;

public sealed class SectionHeaderViewModel
{
    public string? Index { get; init; }
    public string? Eyebrow { get; init; }
    public string Title { get; init; } = string.Empty;
    public string HeadingLevel { get; init; } = "h2";
    public string? Id { get; init; }
}
