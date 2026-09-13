using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class IndustriesIndexModel : PageModel
{
    public void OnGet() => PlaceholderPage.Configure(this, "Industries", "/industries");
}
