using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class PrivacyModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Privacy — Eterna";
        ViewData["Description"] = "Privacy information for Eterna will be published here.";
        ViewData["CanonicalPath"] = "/privacy";
        ViewData["Robots"] = "noindex, nofollow";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
