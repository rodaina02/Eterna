using Microsoft.AspNetCore.Http;

namespace Eterna.Web.Navigation;

public sealed record NavItem(string Label, string Path);

public static class NavigationCatalog
{
    public static IReadOnlyList<NavItem> Primary { get; } =
    [
        new("Work", "/work"),
        new("Services", "/services"),
        new("Technologies", "/technologies"),
        new("Industries", "/industries"),
        new("About", "/about"),
        new("Contact", "/contact")
    ];

    public static readonly NavItem Contact = new("Contact", "/contact");

    public static IReadOnlyList<string> PublicSitemapPaths { get; } =
    [
        "/",
        "/about",
        "/services",
        "/work",
        "/technologies",
        "/industries",
        "/contact"
    ];

    public static bool IsActive(NavItem item, PathString currentPath)
    {
        var path = currentPath.HasValue ? currentPath.Value! : "/";
        return path.Equals(item.Path, StringComparison.OrdinalIgnoreCase)
            || path.StartsWith(item.Path + "/", StringComparison.OrdinalIgnoreCase);
    }

    public static string HeaderBrandVariant(string theme)
    {
        return theme switch
        {
            "paper" => "black-on-white",
            "lime" => "dark-on-lime",
            "black" => "lime-on-black",
            _ => "lime-on-dark"
        };
    }
}
