using FluentValidation;

namespace Eterna.Application.Contacts;

public sealed class ContactFormValidator : AbstractValidator<ContactFormRequest>
{
    public static readonly string[] AllowedServices =
    [
        "Website",
        "Mobile Application",
        "Software System",
        "AI Integration",
        "AI Agent",
        "Automation",
        "Other"
    ];

    public ContactFormValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Company)
            .MaximumLength(160)
            .When(x => !string.IsNullOrWhiteSpace(x.Company));

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .MaximumLength(40)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Service)
            .NotEmpty()
            .Must(service => AllowedServices.Contains(service))
            .WithMessage("Select a valid service.");

        RuleFor(x => x.Budget)
            .MaximumLength(40)
            .When(x => !string.IsNullOrWhiteSpace(x.Budget));

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(x => x.Website)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }
}
