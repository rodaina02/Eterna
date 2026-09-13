using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class ServicesIndexModel : PageModel
{
    public void OnGet() => PlaceholderPage.Configure(this, "Services", "/services");
}
