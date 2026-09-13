using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class IndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Eterna — Building Intelligent Legacies";
        ViewData["Description"] =
            "Eterna is a software and AI solutions company dedicated to building durable digital products and intelligent systems for modern enterprises.";
        ViewData["CanonicalPath"] = "/";
        ViewData["HeaderTheme"] = "dark";
        ViewData["BodyTheme"] = "dark";
        ViewData["MainClass"] = "site-main--foundation";
    }
}
