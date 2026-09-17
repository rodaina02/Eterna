namespace Eterna.Web.Content;

public static class IndustriesContent
{
    public const string Title = "Industries — Eterna";

    public const string Description =
        "Business contexts represented by Eterna's work — commerce, fashion, home, sport, and emerging digital experiences — with one engineering mindset.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "Different",
        "contexts.",
        "One",
        "engineering",
        "mindset."
    ];

    public const string IntroKicker = "Industries & contexts.";

    public const string IntroLede =
        "Software is shaped by the business context it serves. The work in Eterna's portfolio appears across different markets and constraints — the engineering approach stays consistent.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Commerce",
        "Fashion",
        "Home",
        "Sport",
        "Emerging"
    ];

    public static IReadOnlyList<IndustryContext> Contexts { get; } =
    [
        new()
        {
            Id = "commerce",
            Index = "02",
            Title = "Commerce & retail",
            TitleLines = ["Commerce", "& retail."],
            Lede =
                "Several engagements are customer-facing digital storefronts: product discovery, e-commerce, and the systems that hold a shop together. The work is not identical from client to client.",
            Points =
            [
                "E-commerce",
                "Digital storefronts",
                "Product discovery",
                "Customer-facing digital experiences"
            ],
            ClientIds = ["cudds", "atiiq", "nabila-hayel", "reehan-bahaa"],
            Theme = "black",
            Modifier = "commerce",
            WorkLinkText = "Selected work"
        },
        new()
        {
            Id = "fashion",
            Index = "03",
            Title = "Fashion & luxury",
            TitleLines = ["Fashion", "& luxury."],
            Lede =
                "Eterna has worked on digital experiences for fashion and luxury brands — presentation, e-commerce experience, UI/UX, and website architecture. Where stewardship is part of the engagement, that includes ongoing digital infrastructure.",
            Points =
            [
                "Digital presentation",
                "E-commerce experience",
                "UI / UX",
                "Website architecture"
            ],
            ClientIds = ["nabila-hayel", "reehan-bahaa"],
            Theme = "dark",
            Modifier = "fashion"
        },
        new()
        {
            Id = "home-lifestyle",
            Index = "04",
            Title = "Home & lifestyle",
            TitleLines = ["Home", "& lifestyle."],
            Lede =
                "Home and lifestyle contexts in the portfolio are product-heavy: category navigation, visual storytelling, and the digital storefront experience.",
            Points =
            [
                "Product-heavy commerce",
                "Visual storytelling",
                "Category navigation",
                "Digital storefront experience"
            ],
            ClientIds = ["cudds", "atiiq"],
            Theme = "paper",
            Modifier = "home"
        },
        new()
        {
            Id = "sports",
            Index = "05",
            Title = "Sports & athleisure",
            TitleLines = ["Sports", "& athleisure."],
            Lede =
                "Eterna is developing a website and shopping platform in a sports and athleisure context. The product is not a finished public case study.",
            Points =
            [
                "Sports",
                "Athleisure",
                "Shopping platform"
            ],
            ClientIds = ["iron-grip"],
            Theme = "black",
            Modifier = "sports",
            ShowClientStatus = true
        },
        new()
        {
            Id = "creative",
            Index = "06",
            Title = "Creative & emerging brands",
            TitleLines = ["Creative", "& emerging", "brands."],
            Lede =
                "The same engineering approach can support more experimental digital experiences. AfterTale is one such engagement: an animated, creative website currently in development.",
            Points =
            [
                "Creative digital experience",
                "Animated website development",
                "Emerging brand context"
            ],
            ClientIds = ["aftertale"],
            Theme = "dark",
            Modifier = "creative",
            ShowClientStatus = true
        }
    ];

    public static IReadOnlyList<string> PrincipleTitleLines { get; } =
    [
        "The product",
        "changes.",
        "The engineering",
        "principles remain."
    ];

    public const string PrincipleLede =
        "Different businesses, different users, different constraints, different systems. The emphasis stays on the same principles.";

    public static IReadOnlyList<string> Principles { get; } =
    [
        "Clarity",
        "Reliability",
        "Human-centered design",
        "Intelligent technology",
        "Durability"
    ];

    public static IReadOnlyList<string> SelectedTitleLines { get; } =
    [
        "Selected",
        "work."
    ];

    public const string SelectedLede =
        "The contexts above are represented by the same clients as the Work page. Each name leads to that project.";

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Have a context.",
        "We'll build",
        "the system."
    ];

    public static IReadOnlyList<WorkContent.WorkProject> ClientsFor(IndustryContext context) =>
        context.ClientIds
            .Select(id => WorkContent.Clients.First(client => string.Equals(client.Id, id, StringComparison.Ordinal)))
            .ToList();

    public sealed class IndustryContext
    {
        public required string Id { get; init; }
        public required string Index { get; init; }
        public required string Title { get; init; }
        public required IReadOnlyList<string> TitleLines { get; init; }
        public required string Lede { get; init; }
        public required IReadOnlyList<string> Points { get; init; }
        public required IReadOnlyList<string> ClientIds { get; init; }
        public required string Theme { get; init; }
        public required string Modifier { get; init; }
        public string? WorkLinkText { get; init; }
        public bool ShowClientStatus { get; init; }
    }
}
