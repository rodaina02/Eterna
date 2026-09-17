using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Eterna.Web.Tests;

public sealed class ProductionReadinessTests : IClassFixture<ContactWebApplicationFactory>
{
    private readonly ContactWebApplicationFactory _factory;

    public ProductionReadinessTests(ContactWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/", "Eterna — Software, AI & Intelligent Systems")]
    [InlineData("/about", "About — Eterna")]
    [InlineData("/services", "Services — Eterna")]
    [InlineData("/work", "Work — Eterna")]
    [InlineData("/contact", "Contact — Eterna")]
    public async Task Primary_pages_have_unique_titles(string path, string title)
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync(path);
        var rendered = WebUtility.HtmlDecode(
            Regex.Match(html, "<title>([^<]+)</title>", RegexOptions.IgnoreCase).Groups[1].Value);

        Assert.Equal(title, rendered);
        Assert.Contains($"<meta name=\"description\"", html, StringComparison.Ordinal);
        Assert.Contains("og:title", html, StringComparison.Ordinal);
        Assert.Contains("og:description", html, StringComparison.Ordinal);
        Assert.True(Regex.Matches(html, "<h1\\b").Count == 1);
    }

    [Theory]
    [InlineData("/about")]
    [InlineData("/services")]
    [InlineData("/work")]
    [InlineData("/contact")]
    public async Task Inner_pages_do_not_load_homepage_motion_scripts(string path)
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync(path);

        Assert.DoesNotContain("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.scroll.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.portfolio.js", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Homepage_points_at_services_section_anchors()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/");

        Assert.Contains("/services#software-systems", html, StringComparison.Ordinal);
        Assert.Contains("/services#ai-integration", html, StringComparison.Ordinal);
        Assert.Contains("/services#ai-agents", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/services/software-systems", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/services/intelligent-ai-integration", html, StringComparison.Ordinal);
        Assert.DoesNotContain("/services/ai-agents-automation", html, StringComparison.Ordinal);
        Assert.Contains("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.Contains("eterna.scroll.js", html, StringComparison.Ordinal);
        Assert.Contains("eterna.portfolio.js", html, StringComparison.Ordinal);
        Assert.Contains("reehan.png", html, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/not-a-real-page")]
    [InlineData("/work/not-a-real-project")]
    [InlineData("/services/not-a-real-service")]
    [InlineData("/services/software-systems")]
    public async Task Unknown_routes_return_sanitized_404(string path)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(path);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains("Page not found.", body, StringComparison.Ordinal);
        Assert.DoesNotContain("Exception", body, StringComparison.Ordinal);
        Assert.DoesNotContain("stack", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ConnectionStrings", body, StringComparison.Ordinal);
        Assert.DoesNotContain("ApiKey", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Responses_include_frame_and_content_security_headers()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("strict-origin-when-cross-origin", response.Headers.GetValues("Referrer-Policy").Single());
        Assert.Contains("camera=()", response.Headers.GetValues("Permissions-Policy").Single(), StringComparison.Ordinal);

        var csp = response.Headers.GetValues("Content-Security-Policy").Single();
        Assert.Contains("frame-ancestors 'none'", csp, StringComparison.Ordinal);
        Assert.Contains("script-src 'self' 'nonce-", csp, StringComparison.Ordinal);
        Assert.DoesNotContain("upgrade-insecure-requests", csp, StringComparison.Ordinal);

        var nonceMatch = Regex.Match(csp, "nonce-([^']+)");
        Assert.True(nonceMatch.Success);
        Assert.Contains($"nonce=\"{nonceMatch.Groups[1].Value}\"", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Json_ld_is_valid_and_omits_placeholder_urls()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/");
        var json = ExtractJsonLd(html);
        using var document = JsonDocument.Parse(json);
        var graph = document.RootElement.GetProperty("@graph");

        foreach (var node in graph.EnumerateArray())
        {
            Assert.False(node.TryGetProperty("url", out _));
        }

        Assert.DoesNotContain("example.com", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("localhost", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("placeholder", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Canonical_base_url_is_applied_when_configured()
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

        var html = await client.GetStringAsync("/about");
        Assert.Contains("rel=\"canonical\" href=\"https://eterna.example/about\"", html, StringComparison.Ordinal);
        Assert.Contains("og:url\" content=\"https://eterna.example/about\"", html, StringComparison.Ordinal);

        var json = ExtractJsonLd(html);
        using var document = JsonDocument.Parse(json);
        foreach (var node in document.RootElement.GetProperty("@graph").EnumerateArray())
        {
            Assert.Equal("https://eterna.example", node.GetProperty("url").GetString());
        }
    }

    [Fact]
    public async Task Sitemap_lists_only_completed_public_routes()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/sitemap.xml");
        var xml = await response.Content.ReadAsStringAsync();

        Assert.Equal("application/xml; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Contains("</loc>", xml, StringComparison.Ordinal);
        Assert.Contains("/about", xml, StringComparison.Ordinal);
        Assert.Contains("/services", xml, StringComparison.Ordinal);
        Assert.Contains("/work", xml, StringComparison.Ordinal);
        Assert.Contains("/contact", xml, StringComparison.Ordinal);
        Assert.DoesNotContain("/technologies", xml, StringComparison.Ordinal);
        Assert.DoesNotContain("/industries", xml, StringComparison.Ordinal);
        Assert.DoesNotContain("/design-system", xml, StringComparison.Ordinal);
        Assert.DoesNotContain("/privacy", xml, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Robots_allows_indexing_and_points_at_sitemap()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/robots.txt");
        var body = await response.Content.ReadAsStringAsync();

        Assert.StartsWith("text/plain", response.Content.Headers.ContentType?.MediaType, StringComparison.Ordinal);
        Assert.Contains("Allow: /", body, StringComparison.Ordinal);
        Assert.Contains("Sitemap:", body, StringComparison.Ordinal);
        Assert.Contains("/sitemap.xml", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Work_page_links_live_clients_only()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/work");

        Assert.Contains("http://cudds.shop/", html, StringComparison.Ordinal);
        Assert.Contains("https://nabilahayel.com/", html, StringComparison.Ordinal);
        Assert.Contains("https://atiiq.com/", html, StringComparison.Ordinal);
        Assert.Contains("https://reehanbahaa.com/", html, StringComparison.Ordinal);
        Assert.Equal(4, Regex.Matches(html, "work-project__visit").Count);
        Assert.Contains("nabila-hayel-gold-transparent.png", html, StringComparison.Ordinal);
        Assert.Contains("reehan.png", html, StringComparison.Ordinal);
        Assert.Contains("aftertale-wordmark.png", html, StringComparison.Ordinal);
        Assert.Contains("iron-grip-light.png", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Health_endpoint_is_reachable()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static string ExtractJsonLd(string html)
    {
        var match = Regex.Match(
            html,
            "<script[^>]*>(.*?)</script>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        while (match.Success)
        {
            var body = match.Groups[1].Value;
            if (body.Contains("schema.org", StringComparison.OrdinalIgnoreCase))
            {
                return body.Trim();
            }

            match = match.NextMatch();
        }

        Assert.Fail("JSON-LD script was not emitted.");
        return string.Empty;
    }
}
