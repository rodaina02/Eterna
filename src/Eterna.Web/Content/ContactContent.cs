namespace Eterna.Web.Content;

public static class ContactContent
{
    public const string Title = "Contact — Eterna";

    public const string Description =
        "Start a project with Eterna. Share the context of your software, AI, or automation work so we can understand the opportunity.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "Let's build",
        "something",
        "that lasts."
    ];

    public const string IntroLede =
        "Share the context of the project. Eterna uses the details to understand the opportunity — the product, the constraint, and what needs to last.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Projects",
        "Software",
        "AI",
        "Automation"
    ];

    public const string IntakeLede =
        "Tell us what you want to build. Required fields are marked with an asterisk.";

    public const string DetailsLede =
        "The inquiry is a starting point, not a contract. It helps Eterna understand fit before any engagement is shaped.";

    public static IReadOnlyList<string> DetailPoints { get; } =
    [
        "Required: name, email, service, message",
        "Optional: company, phone, budget",
        "Used to understand the product and the constraint"
    ];

    public const string DirectLede =
        "If you already know how you want to reach Eterna, use the channels below.";

    public static IReadOnlyList<string> ProcessTitleLines { get; } =
    [
        "From inquiry",
        "to engagement."
    ];

    public const string ProcessLede =
        "Once a project inquiry is received, the work can move through Eterna's established process — according to what the engagement actually needs. Not every project follows every stage in the same way.";

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Ready when",
        "you are."
    ];

    public const string SuccessTitle = "Project received.";

    public const string SuccessLede =
        "Thank you for reaching out to Eterna. Your project details have been received.";

    public const string ServerErrorMessage =
        "Your project details could not be completed. Please try again.";

    public const string RateLimitedMessage =
        "Too many project submissions were received from this connection. Please wait a few minutes and try again.";

    public const string ValidationSummary = "Please correct the highlighted fields.";
}
