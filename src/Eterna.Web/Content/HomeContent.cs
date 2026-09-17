using Eterna.Web.ViewModels;

namespace Eterna.Web.Content;

public static class HomeContent
{
    public static IReadOnlyList<WorkCardViewModel> KnownWork { get; } =
        WorkContent.ToWorkCards();

    public static IReadOnlyList<PillarViewModel> Pillars { get; } =
    [
        new()
        {
            Index = "01",
            Title = "Enterprise software systems",
            Description = "Web, mobile, APIs and infrastructure built to hold under use.",
            Href = "/services#software-systems",
            LinkText = "Software systems"
        },
        new()
        {
            Index = "02",
            Title = "Intelligent AI integration",
            Description = "Analytics, models and retrieval placed inside the systems people already run.",
            Href = "/services#ai-integration",
            LinkText = "AI integration"
        },
        new()
        {
            Index = "03",
            Title = "AI agents & orchestration",
            Description = "Task-specific agents with human-in-the-loop controls and audit trails.",
            Href = "/services#ai-agents",
            LinkText = "Agents & automation"
        }
    ];

    public static IReadOnlyList<(string Title, string[] Items, string Href)> ServiceGroups { get; } =
    [
        (
            "Software Systems",
            [
                "Web & mobile applications",
                "Responsive websites",
                "E-commerce",
                "Backend APIs",
                "Cloud infrastructure",
                "DevOps & Maintenance"
            ],
            "/services#software-systems"
        ),
        (
            "Intelligent AI Integration",
            [
                "Data Analytics",
                "Machine Learning Models",
                "Smart features",
                "Conversational UIs",
                "RAG search",
                "Workflow automation",
                "Domain-tuned LLMs"
            ],
            "/services#ai-integration"
        ),
        (
            "AI Agents & Automation",
            [
                "Task-specific agents",
                "Micro-agent pipelines",
                "Human-in-the-loop controls",
                "Audit trails",
                "AI Ops & Monitoring"
            ],
            "/services#ai-agents"
        )
    ];

    public static IReadOnlyList<(string Index, string Title, string Text)> Reasons { get; } =
    [
        ("01", "Focused excellence", "A concentrated practice: software systems, AI integration, and agents."),
        ("02", "AI-native architecture", "Intelligence is designed into the system, not attached after delivery."),
        ("03", "Human-centered design", "Tools that augment judgment, creativity, and control."),
        ("04", "Strategic partnership mindset", "Work structured as a digital journey, not a single handover.")
    ];

    public static IReadOnlyList<ProcessStageViewModel> Process { get; } =
    [
        new() { Index = "01", Title = "Strategy & discovery" },
        new() { Index = "02", Title = "Design & architecture" },
        new() { Index = "03", Title = "Development" },
        new() { Index = "04", Title = "AI integration" },
        new() { Index = "05", Title = "Launch & growth" },
        new() { Index = "—", Title = "Evolve", IsContinuation = true }
    ];

    public static IReadOnlyList<(string Title, string[] Items)> TechnologyClusters { get; } =
    [
        ("Languages", ["C#", "JavaScript", "SQL"]),
        ("Application", ["ASP.NET Core", "Razor", ".NET MAUI"]),
        ("Data", ["Entity Framework Core", "Dapper", "SQL Server", "PostgreSQL"]),
        ("AI", ["LLMs", "RAG", "AI Agents", "AI Automation", "MCP", "AI APIs"]),
        ("Cloud & infrastructure", ["Cloud platforms", "Docker"]),
        ("DevOps", ["GitHub Actions"]),
        ("APIs", ["REST APIs"])
    ];

    public static IReadOnlyList<string> HumanIndex { get; } =
    [
        "Augment",
        "Collaborate",
        "Decide",
        "Automate",
        "Observe",
        "Control"
    ];

    public static IReadOnlyList<string> Industries { get; } =
    [
        "Real estate",
        "Healthcare",
        "E-commerce",
        "Retail",
        "Fashion",
        "Sports & athleisure",
        "Professional services"
    ];

    public static IReadOnlyList<string> GovernancePrimary { get; } =
    [
        "Responsibility",
        "Transparency",
        "Explainability",
        "Accountability"
    ];

    public static IReadOnlyList<string> GovernanceSupporting { get; } =
    [
        "Data privacy",
        "Confidentiality",
        "Security",
        "Auditability",
        "Human oversight"
    ];
}
