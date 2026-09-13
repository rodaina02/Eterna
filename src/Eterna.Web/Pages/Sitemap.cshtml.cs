using Eterna.Application.Options;
using Eterna.Web.Navigation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Eterna.Web.Pages;

public sealed class SitemapModel : PageModel
{
    private readonly SiteOptions _site;

    public SitemapModel(IOptions<SiteOptions> site)
    {
        _site = site.Value;
    }

    public IReadOnlyList<string> Paths { get; } = NavigationCatalog.PublicSitemapPaths;

    public string BaseUrl => string.IsNullOrWhiteSpace(_site.CanonicalBaseUrl)
        ? $"{Request.Scheme}://{Request.Host}"
        : _site.CanonicalBaseUrl.TrimEnd('/');

    public IActionResult OnGet()
    {
        Response.ContentType = "application/xml; charset=utf-8";
        return Page();
    }
}
