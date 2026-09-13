namespace Eterna.Web.ViewModels;

public sealed class ButtonViewModel
{
    public string Text { get; init; } = string.Empty;
    public string? Href { get; init; }
    public string Variant { get; init; } = "primary";
    public bool Disabled { get; init; }
    public string? CssClass { get; init; }
    public string? Motion { get; init; }
}
