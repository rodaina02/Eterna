namespace Eterna.Web.ViewModels;

public sealed class PillarViewModel
{
    public string Index { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Href { get; init; } = "/services";
    public string LinkText { get; init; } = "Explore";
}
