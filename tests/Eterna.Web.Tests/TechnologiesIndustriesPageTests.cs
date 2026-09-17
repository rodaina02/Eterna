using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Eterna.Web.Tests;

public sealed class TechnologiesIndustriesPageTests : IClassFixture<ContactWebApplicationFactory>
{
    private readonly ContactWebApplicationFactory _factory;

    public TechnologiesIndustriesPageTests(ContactWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/about")]
    [InlineData("/services")]
    [InlineData("/work")]
    [InlineData("/technologies")]
    [InlineData("/industries")]
    [InlineData("/contact")]
    [InlineData("/privacy")]
    public async Task Public_routes_return_ok(string path)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("/technologies/dotnet")]
    [InlineData("/technologies/ai")]
    [InlineData("/industries/fashion")]
    [InlineData("/services/software-systems")]
    [InlineData("/services/anything")]
    [InlineData("/work/anything")]
    public async Task Detail_routes_remain_not_found(string path)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Technologies_page_is_indexable_editorial_inner_page()
    {
        var client = _factory.CreateClient();
        var html = WebUtility.HtmlDecode(await client.GetStringAsync("/technologies"));

        Assert.Contains("Technologies — Eterna", html, StringComparison.Ordinal);
        Assert.Contains("og:title", html, StringComparison.Ordinal);
        Assert.Contains("og:description", html, StringComparison.Ordinal);
        Assert.DoesNotContain("noindex", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("structural placeholder", html, StringComparison.OrdinalIgnoreCase);
        Assert.True(Regex.Matches(html, "<h1\\b").Count == 1);
        Assert.Contains("aria-current=\"page\">Technologies", html, StringComparison.Ordinal);
        Assert.Contains("/css/pages/technologies.css", html, StringComparison.Ordinal);
        Assert.Contains("/js/pages/technologies.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.scroll.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.portfolio.js", html, StringComparison.Ordinal);
        Assert.Contains("/contact", html, StringComparison.Ordinal);
        Assert.Contains("/services#software-systems", html, StringComparison.Ordinal);
        Assert.Contains("/services#ai-integration", html, StringComparison.Ordinal);
        Assert.Contains("/services#ai-agents", html, StringComparison.Ordinal);
        Assert.DoesNotContain("React", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Next.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Vue", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Angular", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Three.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("WebGL", html, StringComparison.Ordinal);
        Assert.DoesNotContain("schema.org/Product", html, StringComparison.Ordinal);
        Assert.DoesNotContain("schema.org/TechArticle", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Industries_page_reuses_work_clients_and_real_anchors()
    {
        var client = _factory.CreateClient();
        var html = WebUtility.HtmlDecode(await client.GetStringAsync("/industries"));

        Assert.Contains("Industries — Eterna", html, StringComparison.Ordinal);
        Assert.DoesNotContain("noindex", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("structural placeholder", html, StringComparison.OrdinalIgnoreCase);
        Assert.True(Regex.Matches(html, "<h1\\b").Count == 1);
        Assert.Contains("aria-current=\"page\">Industries", html, StringComparison.Ordinal);
        Assert.Contains("/css/pages/industries.css", html, StringComparison.Ordinal);
        Assert.Contains("/js/pages/industries.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.scroll.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.portfolio.js", html, StringComparison.Ordinal);
        Assert.Contains("id=\"commerce\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"fashion\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"home-lifestyle\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"sports\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"creative\"", html, StringComparison.Ordinal);
        Assert.Contains("/work#cudds", html, StringComparison.Ordinal);
        Assert.Contains("/work#nabila-hayel", html, StringComparison.Ordinal);
        Assert.Contains("/work#atiiq", html, StringComparison.Ordinal);
        Assert.Contains("/work#iron-grip", html, StringComparison.Ordinal);
        Assert.Contains("/work#reehan-bahaa", html, StringComparison.Ordinal);
        Assert.Contains("/work#aftertale", html, StringComparison.Ordinal);
        Assert.Equal(6, Regex.Matches(html, "industries-work__link").Count);
        Assert.Contains("In development", html, StringComparison.Ordinal);
        Assert.DoesNotContain("we specialize in fashion", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("cudds.shop", html, StringComparison.Ordinal);
        Assert.DoesNotContain("nabilahayel.com", html, StringComparison.Ordinal);
        Assert.DoesNotContain("atiiq.com", html, StringComparison.Ordinal);
        Assert.DoesNotContain("reehanbahaa.com", html, StringComparison.Ordinal);
        Assert.Contains("/contact", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Homepage_keeps_existing_industry_link_and_does_not_load_inner_page_scripts()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/");

        Assert.Contains("Explore contexts", html, StringComparison.Ordinal);
        Assert.Contains("href=\"/industries\"", html, StringComparison.Ordinal);
        Assert.Contains("href=\"/technologies\"", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/js/pages/technologies.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/js/pages/industries.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/css/pages/technologies.css", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/css/pages/industries.css", html, StringComparison.Ordinal);
        Assert.Contains("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.Contains("eterna.scroll.js", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Robots_does_not_disallow_new_public_routes()
    {
        var client = _factory.CreateClient();
        var body = await client.GetStringAsync("/robots.txt");

        Assert.Contains("Allow: /", body, StringComparison.Ordinal);
        Assert.DoesNotContain("Disallow: /technologies", body, StringComparison.Ordinal);
        Assert.DoesNotContain("Disallow: /industries", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Canonical_base_url_is_applied_to_new_public_pages()
    {
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("Site:CanonicalBaseUrl", "https://eterna.example");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Site:CanonicalBaseUrl"] = "https://eterna.example"
                });
            });
        }).CreateClient();

        var technologies = await client.GetStringAsync("/technologies");
        Assert.Contains("rel=\"canonical\" href=\"https://eterna.example/technologies\"", technologies, StringComparison.Ordinal);
        Assert.Contains("og:url\" content=\"https://eterna.example/technologies\"", technologies, StringComparison.Ordinal);

        var industries = await client.GetStringAsync("/industries");
        Assert.Contains("rel=\"canonical\" href=\"https://eterna.example/industries\"", industries, StringComparison.Ordinal);
        Assert.Contains("og:url\" content=\"https://eterna.example/industries\"", industries, StringComparison.Ordinal);
    }
}
