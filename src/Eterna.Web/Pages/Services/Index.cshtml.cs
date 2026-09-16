using Eterna.Web.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class ServicesIndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = ServicesContent.Title;
        ViewData["Description"] = ServicesContent.Description;
        ViewData["CanonicalPath"] = "/services";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
