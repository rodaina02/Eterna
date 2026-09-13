using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class WorkItemModel : PageModel
{
    public void OnGet(string slug) => PlaceholderPage.Configure(this, "Work", $"/work/{slug}");
}
