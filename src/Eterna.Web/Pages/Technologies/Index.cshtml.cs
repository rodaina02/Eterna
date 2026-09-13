using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class TechnologiesIndexModel : PageModel
{
    public void OnGet() => PlaceholderPage.Configure(this, "Technologies", "/technologies");
}
