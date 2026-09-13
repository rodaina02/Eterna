using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class ContactIndexModel : PageModel
{
    public void OnGet() => PlaceholderPage.Configure(this, "Contact", "/contact");
}
