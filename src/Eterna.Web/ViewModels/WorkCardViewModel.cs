namespace Eterna.Web.ViewModels;

public sealed class WorkCardViewModel
{
    public string Client { get; init; } = string.Empty;
    public string Industry { get; init; } = string.Empty;
    public string Label { get; init; } = "Client";
    public string Index { get; init; } = "";
    public string? Href { get; init; }
    public string? Modifier { get; init; }
    public string? LogoSrc { get; init; }
    public int? LogoWidth { get; init; }
    public int? LogoHeight { get; init; }
}
