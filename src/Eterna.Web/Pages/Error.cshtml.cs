using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class ErrorModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Something went wrong — Eterna";
        ViewData["Description"] = "The requested page could not be completed. Please try again.";
        ViewData["CanonicalPath"] = "/Error";
    }
}
