using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Hosting;

public static class PlaceholderPage
{
    public static void Configure(
        PageModel page,
        string title,
        string path,
        string? description = null)
    {
        page.ViewData["Title"] = $"{title} — Eterna";
        page.ViewData["Description"] = description
            ?? $"The {title.ToLowerInvariant()} page for Eterna is being prepared.";
        page.ViewData["CanonicalPath"] = path;
        page.ViewData["HeaderTheme"] = "paper";
        page.ViewData["BodyTheme"] = "paper";
        page.ViewData["MainClass"] = "site-main--inner";
        page.ViewData["PageHeading"] = title;
    }
}
