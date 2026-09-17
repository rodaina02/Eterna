using Eterna.Web.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class TechnologiesIndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = TechnologiesContent.Title;
        ViewData["Description"] = TechnologiesContent.Description;
        ViewData["CanonicalPath"] = "/technologies";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
