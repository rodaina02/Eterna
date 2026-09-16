using Eterna.Web.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class AboutIndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = AboutContent.Title;
        ViewData["Description"] = AboutContent.Description;
        ViewData["CanonicalPath"] = "/about";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
