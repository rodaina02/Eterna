using Eterna.Web.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class WorkIndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = WorkContent.Title;
        ViewData["Description"] = WorkContent.Description;
        ViewData["CanonicalPath"] = "/work";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
