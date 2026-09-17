namespace Eterna.Web.Content;

public static class TechnologiesContent
{
    public const string Title = "Technologies — Eterna";

    public const string Description =
        "Eterna selects software, data, intelligence, and infrastructure technologies according to the system being built — not as a fixed stack for every project.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "The system",
        "behind the",
        "system."
    ];

    public const string IntroKicker = "Technology is selected around the system — not the other way around.";

    public const string IntroLede =
        "Eterna combines software engineering, infrastructure, data, and intelligent technologies according to the needs of the product being built.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Software",
        "Data",
        "AI",
        "Infrastructure",
        "Experience"
    ];

    public const string EngineeringTitle = "Engineering foundations";

    public const string EngineeringLede =
        "These form part of Eterna's application-engineering foundation for building web applications, APIs, and software systems. They are selected according to the project — not used all at once by default.";

    public static IReadOnlyList<TechItem> Engineering { get; } =
    [
        new(".NET / ASP.NET Core"),
        new("C#"),
        new("Razor Pages"),
        new("REST APIs"),
        new("Entity Framework Core"),
        new("Dapper")
    ];

    public const string DataTitle = "Data & systems";

    public const string DataLede =
        "Some of what follows are data technologies. Others are capabilities that sit on top of them. The distinction matters: a capability is not a product name.";

    public static IReadOnlyList<TechItem> Data { get; } =
    [
        new("PostgreSQL"),
        new("SQL Server"),
        new("Data Analytics", "Capability"),
        new("Data-driven systems", "Capability"),
        new("Redis")
    ];

    public const string IntelligenceTitle = "Intelligent systems";

    public const string IntelligenceLede =
        "When a product needs intelligence, it is placed inside the system people already use — as models, retrieval, conversation, and defined action — with control remaining part of the design.";

    public static IReadOnlyList<TechItem> Intelligence { get; } =
    [
        new("Machine learning"),
        new("LLMs"),
        new("RAG"),
        new("Conversational interfaces"),
        new("Smart features"),
        new("Domain-tuned models"),
        new("Workflow automation"),
        new("AI agents")
    ];

    public static IReadOnlyList<TechFlowStep> IntelligenceFlow { get; } =
    [
        new("Data", "/services#software-systems"),
        new("Intelligence", "/services#ai-integration"),
        new("Action", "/services#ai-agents")
    ];

    public const string IntelligenceFlowNote =
        "Data, intelligence, and action are layers of one offering. They can be combined according to the project.";

    public const string InfrastructureTitle = "Infrastructure & delivery";

    public const string InfrastructureLede =
        "The work continues past the interface: infrastructure, delivery, messaging, and the operational layer that keeps a system running.";

    public static IReadOnlyList<TechItem> Infrastructure { get; } =
    [
        new("Cloud infrastructure", "Capability"),
        new("DevOps", "Capability"),
        new("CI/CD", "Capability"),
        new("RabbitMQ"),
        new("MassTransit"),
        new("Redis"),
        new("Observability", "Capability"),
        new("Reverse proxy / gateway architecture", "Capability")
    ];

    public const string ExperienceTitle = "Digital experience";

    public const string ExperienceLede =
        "Frontend technology is used to translate product requirements into usable digital experiences — not as a comparison of frameworks.";

    public static IReadOnlyList<TechItem> Experience { get; } =
    [
        new("HTML"),
        new("CSS"),
        new("JavaScript"),
        new("Responsive interfaces", "Capability"),
        new("UI / UX", "Capability"),
        new("GSAP / ScrollTrigger")
    ];

    public const string SystemTitle = "Technology as a system";

    public static IReadOnlyList<string> SystemTitleLines { get; } =
    [
        "Assembled",
        "around the",
        "product."
    ];

    public const string SystemLede =
        "These layers can work together. They are selected around the product being built — not every project follows this sequence, and not every layer is required.";

    public static IReadOnlyList<string> SystemLayers { get; } =
    [
        "Experience",
        "Application",
        "Data",
        "Intelligence",
        "Infrastructure"
    ];

    public const string SystemProduct = "The product";

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Build the",
        "system behind",
        "the idea."
    ];

    public sealed record TechItem(string Name, string Kind = "Technology");

    public sealed record TechFlowStep(string Name, string Href);
}
