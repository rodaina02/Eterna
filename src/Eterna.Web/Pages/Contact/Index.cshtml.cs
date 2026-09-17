using Eterna.Application.Abstractions;
using Eterna.Application.Contacts;
using Eterna.Web.Content;
using Eterna.Web.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

[RequestFormLimits(ValueCountLimit = 16, ValueLengthLimit = 5000)]
[RequestSizeLimit(64 * 1024)]
public sealed class ContactIndexModel : PageModel
{
    private readonly IContactSubmissionService _submissions;

    public ContactIndexModel(IContactSubmissionService submissions)
    {
        _submissions = submissions;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Company { get; set; }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string? Phone { get; set; }

    [BindProperty]
    public string Service { get; set; } = string.Empty;

    [BindProperty]
    public string? Budget { get; set; }

    [BindProperty]
    public string Message { get; set; } = string.Empty;

    [BindProperty]
    public string? Website { get; set; }

    public string FormStatus { get; private set; } = ContactFormStatuses.Default;

    public IReadOnlyList<string> Services { get; } = ContactFormValidator.AllowedServices;

    public void OnGet()
    {
        ConfigurePage();
        FormStatus = TempData[ContactFormStatuses.TempDataKey] as string
            ?? ContactFormStatuses.Default;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        ConfigurePage();
        ModelState.Clear();

        var request = new ContactFormRequest
        {
            Name = Name ?? string.Empty,
            Company = Company,
            Email = Email ?? string.Empty,
            Phone = Phone,
            Service = Service ?? string.Empty,
            Budget = Budget,
            Message = Message ?? string.Empty,
            Website = Website
        };

        var result = await _submissions.SubmitAsync(
            request,
            new ContactSubmitContext(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString()),
            cancellationToken);

        switch (result.Outcome)
        {
            case ContactSubmitOutcome.Accepted:
                TempData[ContactFormStatuses.TempDataKey] = ContactFormStatuses.Success;
                return RedirectToPage("/Contact/Index");

            case ContactSubmitOutcome.ValidationFailed:
                foreach (var (key, messages) in result.Errors)
                {
                    foreach (var message in messages)
                    {
                        ModelState.AddModelError(key, message);
                    }
                }

                FormStatus = ContactFormStatuses.Default;
                return Page();

            default:
                FormStatus = ContactFormStatuses.ServerError;
                return Page();
        }
    }

    public bool FieldInvalid(string field)
    {
        return ModelState[field]?.Errors.Count > 0;
    }

    public string? FieldError(string field)
    {
        return ModelState[field]?.Errors.FirstOrDefault()?.ErrorMessage;
    }

    private void ConfigurePage()
    {
        ViewData["Title"] = ContactContent.Title;
        ViewData["Description"] = ContactContent.Description;
        ViewData["CanonicalPath"] = "/contact";
        ViewData["HeaderTheme"] = "paper";
        ViewData["BodyTheme"] = "paper";
        ViewData["MainClass"] = "site-main--inner";
    }
}
