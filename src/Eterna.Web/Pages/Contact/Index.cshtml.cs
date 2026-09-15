using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class ContactIndexModel : PageModel
{
    public void OnGet() => PlaceholderPage.Configure(this, "Contact", "/contact");

    public IActionResult OnPost()
    {
        PlaceholderPage.Configure(this, "Contact", "/contact");
        return Page();
    }
}
