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
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Enter your name.")
            .MaximumLength(120)
            .WithMessage("Use 120 characters or fewer.");

        RuleFor(x => x.Company)
            .MaximumLength(160)
            .WithMessage("Use 160 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.Company));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Enter your email.")
            .MaximumLength(254)
            .WithMessage("Use 254 characters or fewer.")
            .EmailAddress()
            .WithMessage("Enter a valid email.");

        RuleFor(x => x.Phone)
            .MaximumLength(40)
            .WithMessage("Use 40 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Service)
            .NotEmpty()
            .WithMessage("Select a service.")
            .Must(service => AllowedServices.Contains(service))
            .WithMessage("Select a valid service.");

        RuleFor(x => x.Budget)
            .MaximumLength(40)
            .WithMessage("Use 40 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.Budget));

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Enter a message.")
            .MaximumLength(4000)
            .WithMessage("Use 4,000 characters or fewer.");

        RuleFor(x => x.Website)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }
}
