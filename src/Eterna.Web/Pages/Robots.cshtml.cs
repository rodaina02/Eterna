using Eterna.Application.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Eterna.Web.Pages;

public sealed class RobotsModel : PageModel
{
    private readonly SiteOptions _site;

    public RobotsModel(IOptions<SiteOptions> site)
    {
        _site = site.Value;
    }

    public string SitemapUrl => string.IsNullOrWhiteSpace(_site.CanonicalBaseUrl)
        ? $"{Request.Scheme}://{Request.Host}/sitemap.xml"
        : $"{_site.CanonicalBaseUrl.TrimEnd('/')}/sitemap.xml";

    public IActionResult OnGet()
    {
        Response.ContentType = "text/plain; charset=utf-8";
        return Page();
    }
}
