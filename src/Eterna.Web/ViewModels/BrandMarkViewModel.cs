namespace Eterna.Web.ViewModels;

public sealed class BrandMarkViewModel
{
    public string Variant { get; init; } = "lime-on-dark";
    public string? Alt { get; init; }
    public string? CssClass { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public bool Lazy { get; init; }
}
