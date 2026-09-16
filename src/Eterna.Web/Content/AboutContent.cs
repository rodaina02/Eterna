namespace Eterna.Web.Content;

public static class AboutContent
{
    public const string Title = "About — Eterna";

    public const string Description =
        "Eterna is a Software and AI Solutions company dedicated to building durable digital products and intelligent systems for modern enterprises.";

    public const string IntroKicker = "About Eterna.";

    public static IReadOnlyList<string> IntroTitleLines { get; } =
    [
        "Building",
        "systems",
        "that endure."
    ];

    public const string IntroLede =
        "Eterna is a Software and AI Solutions company dedicated to building durable digital products and intelligent systems for modern enterprises.";

    public static IReadOnlyList<string> IntroMeta { get; } =
    [
        "Software",
        "AI",
        "Systems",
        "Automation"
    ];

    public const string WhoLede =
        "A software and AI solutions company focused on durable digital products and intelligent systems — combining technology with human-centered design.";

    public const string WhoBody =
        "The practice covers premium web experiences, mobile applications, software systems, AI-enabled solutions, intelligent automation, decision-support, and advanced UX. The approach is human-centric: systems designed to work alongside people.";

    public static IReadOnlyList<string> Practices { get; } =
    [
        "Premium web experiences",
        "Mobile applications",
        "Software systems",
        "AI-enabled solutions",
        "Intelligent automation",
        "Decision-support",
        "Advanced UX"
    ];

    public static IReadOnlyList<string> HumanStatement { get; } =
    [
        "AI should",
        "augment",
        "human potential.",
        "Not simply replace it."
    ];

    public const string HumanLede =
        "Eterna designs intelligent systems that simplify complexity, amplify creativity, improve decision-making, and help businesses operate more effectively — while preserving human oversight and judgment.";

    public static IReadOnlyList<string> HumanPrinciples { get; } =
    [
        "Intelligence",
        "Creativity",
        "Productivity",
        "Control",
        "Responsibility",
        "Transparency"
    ];

    public const string Vision =
        "Redefining the future with intelligent software and AI agents that empower, transform, and endure.";

    public const string Mission =
        "Deliver cutting-edge websites and mobile applications that help businesses thrive, while leading the shift into AI Agent development by creating systems that think, act, and evolve alongside humans.";

    public static IReadOnlyList<string> MissionGrounding { get; } =
    [
        "Ethics",
        "Resilience",
        "Impact"
    ];

    public static IReadOnlyList<(string Title, string Text)> Values { get; } =
    [
        ("Resilience", "We thrive in uncertainty and turn challenges into growth."),
        ("Innovation", "We take risks, think differently, and lead with creativity."),
        ("Legacy", "We build technology that lasts, creating impact that lives beyond us."),
        ("Human-Centered", "We design technology that empowers and amplifies human potential rather than limiting it.")
    ];

    public const string MarkLede =
        "The Eterna mark is two forms working as one: a forward-leaning curve for progress and evolution, and a grounded form for structure and reliability. Together they stand for integrated, future-focused technology.";

    public static IReadOnlyList<(string Title, string Text)> MarkReadings { get; } =
    [
        ("Progress", "The forward-leaning form — progress and evolution."),
        ("Structure", "The grounded form — structure and reliability."),
        ("Eterna", "The two together — integrated, future-focused technology.")
    ];

    public const string GovernanceTitle = "Intelligence with control.";

    public const string GovernanceLede =
        "Intelligence should remain understandable, reviewable and controllable. Governance is part of how Eterna designs systems — not an afterthought.";

    public static IReadOnlyList<(string Index, string Title, string Text)> Governance { get; } =
    [
        ("01", "Responsibility & control", "Systems remain under human responsibility, with control designed in from the start."),
        ("02", "Transparency & explainability", "Intelligence should be understandable. Decisions should be possible to inspect and explain."),
        ("03", "Privacy & security", "Data privacy, confidentiality and security are treated as product requirements."),
        ("04", "Auditability & traceability", "Work should be reviewable: audit trails, traceability, and records that can be examined."),
        ("05", "Human-in-the-loop", "Automation assists judgment. People remain in the loop where it matters.")
    ];

    public static IReadOnlyList<string> LegacySequence { get; } =
    [
        "Technology",
        "Intelligence",
        "Human capability",
        "Responsibility",
        "Longevity",
        "Legacy"
    ];

    public static IReadOnlyList<string> LegacyTitleLines { get; } =
    [
        "Build technology",
        "that lasts."
    ];

    public const string LegacyLede =
        "Eterna builds durable digital products and intelligent systems so that capability can compound: software that holds, intelligence that remains controllable, and tools that extend human potential over time. That is the work of building intelligent legacies.";

    public static IReadOnlyList<string> CtaTitleLines { get; } =
    [
        "Ready to build",
        "what lasts?"
    ];
}
