using Eterna.Web.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class IndustriesIndexModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = IndustriesContent.Title;
        ViewData["Description"] = IndustriesContent.Description;
        ViewData["CanonicalPath"] = "/industries";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
