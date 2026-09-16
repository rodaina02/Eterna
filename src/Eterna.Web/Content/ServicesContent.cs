namespace Eterna.Web.Content;

public static class ServicesContent
{
    public const string Title = "Services — Eterna";

    public const string Description =
        "Eterna builds software systems, intelligent AI integration, and AI-agent automation into durable digital products.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "We build the",
        "system behind",
        "the experience."
    ];

    public const string IntroLede =
        "Eterna combines software engineering, intelligent AI integration, and AI-agent systems into durable digital products.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Software",
        "AI",
        "Agents",
        "Automation"
    ];

    public static IReadOnlyList<string> OverviewTitleLines { get; } =
    [
        "From digital foundations",
        "to systems that act."
    ];

    public const string ConnectLede =
        "The three practices are layers of one offering. They can be combined according to the project — not every engagement requires every layer.";

    public static IReadOnlyList<string> ConnectTitleLines { get; } =
    [
        "Layers",
        "of one",
        "system."
    ];

    public const string ProcessLede =
        "Work is structured as a digital journey, not a single handover — from strategy and discovery through launch and growth.";

    public static IReadOnlyList<string> ProcessTitleLines { get; } =
    [
        "End-to-end",
        "technology."
    ];

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Have a system",
        "to build?"
    ];

    public static IReadOnlyList<PracticeOverview> Practices { get; } =
    [
        new()
        {
            Id = "software-systems",
            Index = "01",
            SectionIndex = "03",
            Title = "Software Systems",
            Layer = "Systems",
            Descriptor = "Foundation",
            Overview = "Applications, platforms, APIs, cloud infrastructure.",
            TitleLines =
            [
                "Software",
                "that holds",
                "the whole",
                "system together."
            ],
            Lede = "Web, mobile, APIs and infrastructure built to hold under use — the operational foundation of the product.",
            Theme = "paper",
            Modifier = "software",
            Capabilities =
            [
                new("01", "Web & mobile applications"),
                new("02", "Responsive websites"),
                new("03", "E-commerce"),
                new("04", "Backend APIs"),
                new("05", "Cloud infrastructure"),
                new("06", "DevOps & Maintenance")
            ]
        },
        new()
        {
            Id = "ai-integration",
            Index = "02",
            SectionIndex = "04",
            Title = "Intelligent AI Integration",
            Layer = "Intelligence",
            Descriptor = "Intelligence",
            Overview = "Intelligence embedded into existing products and workflows.",
            TitleLines =
            [
                "Intelligence",
                "as part of",
                "the product."
            ],
            Lede = "Analytics, models and retrieval placed inside the systems people already run — intelligence as part of the product, not a separate novelty.",
            Theme = "dark",
            Modifier = "ai",
            Capabilities =
            [
                new("01", "Data Analytics"),
                new("02", "Machine Learning Models"),
                new("03", "Smart features"),
                new("04", "Conversational UIs"),
                new("05", "RAG search"),
                new("06", "Workflow automation"),
                new("07", "Domain-tuned LLMs")
            ]
        },
        new()
        {
            Id = "ai-agents",
            Index = "03",
            SectionIndex = "05",
            Title = "AI Agents & Automation",
            Layer = "Agents",
            Descriptor = "Action",
            Overview = "Systems that can perform defined tasks with control.",
            TitleLines =
            [
                "Systems",
                "that can",
                "act."
            ],
            Lede = "Task-specific agents with human-in-the-loop controls and audit trails — systems that can perform defined work while remaining under human responsibility.",
            Theme = "lime",
            Modifier = "agents",
            Capabilities =
            [
                new("01", "Task-specific agents"),
                new("02", "Micro-agent pipelines"),
                new("03", "Human-in-the-loop controls", true),
                new("04", "Audit trails"),
                new("05", "AI Ops & Monitoring")
            ]
        }
    ];

    public static IReadOnlyList<ProcessStage> Process { get; } =
    [
        new("01", "Strategy & discovery", "The work begins with the system, the constraints, and what needs to last."),
        new("02", "Design & architecture", "Experience and structure are designed together so the product can hold under use."),
        new("03", "Development", "Applications, platforms, APIs, and infrastructure are built as the operational foundation."),
        new("04", "AI integration", "Intelligence is placed inside the systems people already run."),
        new("05", "Launch & growth", "Delivery continues after launch — including DevOps, maintenance, and systems that can evolve.")
    ];

    public sealed record PracticeOverview
    {
        public required string Id { get; init; }
        public required string Index { get; init; }
        public required string SectionIndex { get; init; }
        public required string Title { get; init; }
        public required string Layer { get; init; }
        public required string Descriptor { get; init; }
        public required string Overview { get; init; }
        public required IReadOnlyList<string> TitleLines { get; init; }
        public required string Lede { get; init; }
        public required string Theme { get; init; }
        public required string Modifier { get; init; }
        public required IReadOnlyList<PracticeCapability> Capabilities { get; init; }
    }

    public sealed record PracticeCapability(string Index, string Name, bool Emphasis = false);

    public sealed record ProcessStage(string Index, string Title, string Text);
}
