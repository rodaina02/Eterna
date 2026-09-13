namespace Eterna.Web.ViewModels;

public sealed class NavLinkViewModel
{
    public string Label { get; init; } = string.Empty;
    public string Path { get; init; } = "/";
    public bool IsActive { get; init; }
    public string CssClass { get; init; } = "nav-link";
}
