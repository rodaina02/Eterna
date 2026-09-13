using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class StatusCodeModel : PageModel
{
    public int Code { get; private set; } = 404;
    public string Heading { get; private set; } = "Page not found.";
    public string Message { get; private set; } = "The page you requested does not exist.";

    public void OnGet([FromQuery] int code = 404)
    {
        Code = code;

        (Heading, Message) = code switch
        {
            404 => ("Page not found.", "The page you requested does not exist."),
            429 => ("Please pause.", "Too many requests were received. Try again shortly."),
            _ => ("Something went wrong.", "The request could not be completed.")
        };

        ViewData["Title"] = $"{Heading} — Eterna";
        ViewData["Description"] = Message;
        ViewData["CanonicalPath"] = "/StatusCode";
        ViewData["Robots"] = "noindex, nofollow";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
