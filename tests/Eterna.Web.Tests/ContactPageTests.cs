using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Eterna.Web.Tests;

public sealed class ContactPageTests : IClassFixture<ContactWebApplicationFactory>
{
    private readonly ContactWebApplicationFactory _factory;

    public ContactPageTests(ContactWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Contact_page_renders_editorial_form()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/contact");

        Assert.Contains("<h1", html, StringComparison.Ordinal);
        Assert.True(Regex.Matches(html, "<h1\\b").Count == 1);
        Assert.Contains("that lasts.", html, StringComparison.Ordinal);
        Assert.Contains("Start a project", html, StringComparison.Ordinal);
        Assert.Contains("name=\"__RequestVerificationToken\"", html, StringComparison.Ordinal);
        Assert.Contains("name=\"Name\"", html, StringComparison.Ordinal);
        Assert.Contains("name=\"Website\"", html, StringComparison.Ordinal);
        Assert.Contains("contact-form__alt", html, StringComparison.Ordinal);
        Assert.Contains("/css/pages/contact.css", html, StringComparison.Ordinal);
        Assert.Contains("/js/pages/contact.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.hero.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.scroll.js", html, StringComparison.Ordinal);
        Assert.DoesNotContain("eterna.portfolio.js", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Post_without_antiforgery_is_rejected()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.PostAsync("/contact", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Name"] = "Amira Hassan",
            ["Email"] = "amira@example.com",
            ["Service"] = "Website",
            ["Message"] = "A durable storefront."
        }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Invalid_post_stays_on_form_with_field_errors()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/contact");
        var token = ExtractAntiforgery(html);

        var response = await client.PostAsync("/contact", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["Name"] = "",
            ["Email"] = "not-an-email",
            ["Service"] = "",
            ["Message"] = ""
        }));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Enter your name.", body, StringComparison.Ordinal);
        Assert.Contains("Enter a valid email.", body, StringComparison.Ordinal);
        Assert.Contains("is-invalid", body, StringComparison.Ordinal);
        Assert.DoesNotContain("Project received.", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Honeypot_post_does_not_reveal_detection()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/contact");
        var token = ExtractAntiforgery(html);

        var response = await client.PostAsync("/contact", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["Name"] = "Amira Hassan",
            ["Email"] = "amira@example.com",
            ["Service"] = "Website",
            ["Message"] = "A durable storefront.",
            ["Website"] = "https://spam.example"
        }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("honeypot", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("bot", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Excess_contact_posts_show_a_friendly_rate_limit_message()
    {
        await using var factory = new ContactWebApplicationFactory
        {
            ContactPermitLimit = 2
        };
        var client = factory.CreateClient();
        var html = await client.GetStringAsync("/contact");
        var token = ExtractAntiforgery(html);

        HttpResponseMessage? last = null;
        for (var i = 0; i < 3; i++)
        {
            last = await client.PostAsync("/contact", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = token,
                ["Name"] = "Amira Hassan",
                ["Email"] = "amira@example.com",
                ["Service"] = "Website",
                ["Message"] = "A durable storefront."
            }));
        }

        Assert.NotNull(last);
        var body = await last!.Content.ReadAsStringAsync();
        Assert.Contains("Too many project submissions", body, StringComparison.Ordinal);
        Assert.DoesNotContain("RateLimiter", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Unconfigured_delivery_does_not_claim_success()
    {
        var client = _factory.CreateClient();
        var html = await client.GetStringAsync("/contact");
        var token = ExtractAntiforgery(html);

        var response = await client.PostAsync("/contact", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["Name"] = "Amira Hassan",
            ["Email"] = "amira@example.com",
            ["Service"] = "Website",
            ["Message"] = "A durable storefront."
        }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("Project received.", body, StringComparison.Ordinal);
        Assert.Contains("could not be completed", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Inner_pages_do_not_load_contact_assets()
    {
        var client = _factory.CreateClient();
        foreach (var path in new[] { "/", "/about", "/services", "/work" })
        {
            var html = await client.GetStringAsync(path);
            Assert.DoesNotContain("/css/pages/contact.css", html, StringComparison.Ordinal);
            Assert.DoesNotContain("/js/pages/contact.js", html, StringComparison.Ordinal);
        }
    }

    private static string ExtractAntiforgery(string html)
    {
        var match = Regex.Match(
            html,
            "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"",
            RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            match = Regex.Match(
                html,
                "value=\"([^\"]+)\"[^>]*name=\"__RequestVerificationToken\"",
                RegexOptions.IgnoreCase);
        }

        Assert.True(match.Success, "Antiforgery token was not emitted.");
        return match.Groups[1].Value;
    }
}

public sealed class ContactWebApplicationFactory : WebApplicationFactory<Program>
{
    public int ContactPermitLimit { get; init; } = 100;

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", "Host=127.0.0.1;Port=1;Database=eterna_test;Username=x;Password=x");
        builder.UseSetting("Contact:IpHashSecret", "web-test-secret");
        builder.UseSetting("Resend:ApiKey", "");
        builder.UseSetting("RateLimiting:ContactPermitLimit", ContactPermitLimit.ToString());
        builder.UseSetting("RateLimiting:ContactWindowMinutes", "10");
        builder.UseSetting("RateLimiting:GlobalPermitLimit", "1000");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=127.0.0.1;Port=1;Database=eterna_test;Username=x;Password=x",
                ["Contact:IpHashSecret"] = "web-test-secret",
                ["Resend:ApiKey"] = "",
                ["RateLimiting:ContactPermitLimit"] = ContactPermitLimit.ToString(),
                ["RateLimiting:ContactWindowMinutes"] = "10",
                ["RateLimiting:GlobalPermitLimit"] = "1000"
            });
        });
    }
}
