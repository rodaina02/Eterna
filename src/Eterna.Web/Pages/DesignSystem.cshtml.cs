using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class DesignSystemModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Design System — Eterna";
        ViewData["Description"] = "Internal visual foundation for the Eterna website.";
        ViewData["CanonicalPath"] = "/design-system";
        ViewData["Robots"] = "noindex, nofollow";
        ViewData["HeaderTheme"] = "black";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--document";
    }
}
