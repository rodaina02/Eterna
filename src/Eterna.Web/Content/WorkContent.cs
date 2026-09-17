using Eterna.Web.ViewModels;

namespace Eterna.Web.Content;

public static class WorkContent
{
    public const string Title = "Work — Eterna";

    public const string Description =
        "Selected client work from Eterna — digital products across commerce, fashion, lifestyle, sports, and emerging digital experiences.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "Selected",
        "work."
    ];

    public const string IntroLede =
        "Eterna builds and evolves digital products across commerce, fashion, lifestyle, sports, and emerging digital experiences.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Selected clients",
        "Digital products",
        "Web",
        "E-commerce",
        "AI"
    ];

    public static IReadOnlyList<string> IndexTitleLines { get; } =
    [
        "A curated",
        "body of work."
    ];

    public static IReadOnlyList<string> ProcessTitleLines { get; } =
    [
        "How we work",
        "with clients."
    ];

    public const string ProcessLede =
        "Eterna can enter a project at different points — an existing product that needs repair, a website that needs structural redesign, a migration or infrastructure transition, a new digital experience, or a product still in development.";

    public static IReadOnlyList<string> Engagements { get; } =
    [
        "Troubleshoot",
        "Rebuild",
        "Redesign",
        "Migrate",
        "Manage",
        "Develop"
    ];

    public static IReadOnlyList<string> EntryPoints { get; } =
    [
        "Existing product needing repair",
        "Existing website needing structural redesign",
        "Migration and infrastructure transition",
        "New digital experience",
        "Product or platform currently in development"
    ];

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Your next",
        "project starts",
        "here."
    ];

    public static IReadOnlyList<WorkProject> Clients { get; } =
    [
        new()
        {
            Id = "cudds",
            Index = "01",
            SectionIndex = "03",
            Name = "CUDDS",
            CardIndustry = "Bedding & Home Essentials",
            Context = "Bedding / Home / E-commerce",
            IndexContext = "E-commerce",
            IndexWork = "Website rebuild / redesign",
            Contributions =
            [
                "Troubleshooting and bug fixing",
                "Website structure rebuild",
                "Website redesign"
            ],
            Summary =
                "We entered an existing digital product, resolved its underlying issues, rebuilt its structure, and redesigned the experience.",
            Theme = "paper",
            Modifier = "cudds",
            WebsiteUrl = "http://cudds.shop/",
            SectionLogo = new WorkLogo("/images/clients/cudds-dark.png", 1886, 623),
            IndexLogo = new WorkLogo("/images/clients/cudds-light.png", 1886, 623),
            DarkLogo = new WorkLogo("/images/clients/cudds-light.png", 1886, 623)
        },
        new()
        {
            Id = "nabila-hayel",
            Index = "02",
            SectionIndex = "04",
            Name = "Nabila Hayel",
            CardIndustry = "Luxury Haute Couture",
            Context = "Fashion / E-commerce",
            IndexContext = "Fashion / E-commerce",
            IndexWork = "Migration / stewardship",
            Contributions =
            [
                "Website migration",
                "Website redesign",
                "Server hosting",
                "Server management",
                "Domain hosting",
                "Website health checkups"
            ],
            Summary =
                "The relationship includes both digital transformation and ongoing infrastructure stewardship — migrating and redesigning the site, then continuing to host, manage, and monitor it.",
            Theme = "dark",
            Modifier = "nabila",
            WebsiteUrl = "https://nabilahayel.com/",
            SectionLogo = new WorkLogo("/images/clients/nabila-hayel-gold-transparent.png", 1258, 1178),
            IndexLogo = new WorkLogo("/images/clients/nabila-hayel-gold-transparent.png", 1258, 1178),
            DarkLogo = new WorkLogo("/images/clients/nabila-hayel-gold-transparent.png", 1258, 1178)
        },
        new()
        {
            Id = "atiiq",
            Index = "03",
            SectionIndex = "05",
            Name = "ATIIQ",
            CardIndustry = "Handmade Crafts",
            Context = "Handmade / Home / E-commerce",
            IndexContext = "Handmade / Home",
            IndexWork = "Website recreation",
            Contributions =
            [
                "Website recreation",
                "Website design recreation",
                "Digital experience implementation"
            ],
            Summary = "Eterna recreated the website — its design and the digital experience.",
            Theme = "paper",
            Modifier = "atiiq",
            WebsiteUrl = "https://atiiq.com/",
            SectionLogo = new WorkLogo("/images/clients/atiiq-dark.png", 505, 638),
            IndexLogo = new WorkLogo("/images/clients/atiiq-light.png", 1485, 1959),
            DarkLogo = new WorkLogo("/images/clients/atiiq-light.png", 1485, 1959)
        },
        new()
        {
            Id = "iron-grip",
            Index = "04",
            SectionIndex = "06",
            Name = "IRON GRIP",
            CardIndustry = "Sports & Athleisure",
            Context = "Sports / Athleisure / Shopping",
            IndexContext = "Sports / Athleisure",
            IndexWork = "In development",
            Contributions =
            [
                "Website development",
                "Shopping platform development"
            ],
            Summary = "Eterna is developing the website and shopping platform.",
            Theme = "black",
            Modifier = "iron",
            Status = "In development",
            SectionLogo = new WorkLogo("/images/clients/iron-grip-light.png", 806, 1206),
            IndexLogo = new WorkLogo("/images/clients/iron-grip-light.png", 806, 1206),
            DarkLogo = new WorkLogo("/images/clients/iron-grip-light.png", 806, 1206)
        },
        new()
        {
            Id = "reehan-bahaa",
            Index = "05",
            SectionIndex = "07",
            Name = "Reehan Bahaa",
            CardIndustry = "Fashion",
            Context = "Fashion / E-commerce",
            IndexContext = "Fashion / E-commerce",
            IndexWork = "Redesign / UI/UX",
            Contributions =
            [
                "Website redesign",
                "UI/UX enhancements"
            ],
            Summary = "Eterna redesigned the website and enhanced the UI/UX.",
            Theme = "dark",
            Modifier = "reehan",
            WebsiteUrl = "https://reehanbahaa.com/",
            SectionLogo = new WorkLogo("/images/clients/reehan.png", 994, 329),
            IndexLogo = new WorkLogo("/images/clients/reehan.png", 994, 329),
            DarkLogo = new WorkLogo("/images/clients/reehan.png", 994, 329)
        },
        new()
        {
            Id = "aftertale",
            Index = "06",
            SectionIndex = "08",
            Name = "AfterTale",
            CardIndustry = "Business development & strategy",
            Context = "Business development & strategy",
            IndexContext = "Business development",
            IndexWork = "In development",
            Contributions =
            [
                "Animated website development",
                "Creative website development"
            ],
            Summary = "Eterna is developing an animated and creative website.",
            Theme = "dark",
            Modifier = "aftertale",
            Status = "In development",
            SectionLogo = new WorkLogo("/images/clients/aftertale-square.png", 351, 142),
            IndexLogo = new WorkLogo("/images/clients/aftertale-wordmark.png", 309, 84),
            DarkLogo = new WorkLogo("/images/clients/aftertale-wordmark.png", 309, 84)
        }
    ];

    public static IReadOnlyList<WorkCardViewModel> ToWorkCards() =>
        Clients
            .Select((project, index) => ToWorkCard(project, index))
            .ToList();

    public static WorkCardViewModel ToWorkCard(WorkProject project, int index) =>
        new()
        {
            Client = project.Name,
            Industry = project.CardIndustry,
            Label = "Selected work",
            Index = (index + 1).ToString("00"),
            Href = $"/work#{project.Id}",
            Modifier = project.Modifier,
            LogoSrc = project.DarkLogo?.Src,
            LogoWidth = project.DarkLogo?.Width,
            LogoHeight = project.DarkLogo?.Height
        };

    public sealed class WorkProject
    {
        public required string Id { get; init; }
        public required string Index { get; init; }
        public required string SectionIndex { get; init; }
        public required string Name { get; init; }
        public required string CardIndustry { get; init; }
        public required string Context { get; init; }
        public required string IndexContext { get; init; }
        public required string IndexWork { get; init; }
        public required IReadOnlyList<string> Contributions { get; init; }
        public required string Summary { get; init; }
        public required string Theme { get; init; }
        public required string Modifier { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? Status { get; init; }
        public WorkLogo? SectionLogo { get; init; }
        public WorkLogo? IndexLogo { get; init; }
        public WorkLogo? DarkLogo { get; init; }
    }

    public sealed record WorkLogo(string Src, int Width, int Height);
}
