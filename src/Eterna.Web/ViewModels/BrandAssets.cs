namespace Eterna.Web.ViewModels;

public static class BrandAssets
{
    public static BrandAsset Resolve(string variant)
    {
        return variant switch
        {
            "lime-on-dark" or "dark" or "nav" =>
                new("/images/brand/mark-lime-on-dark.png", 72, 72, "Eterna"),
            "lime-compact" or "compact" =>
                new("/images/brand/mark-lime-no-r.png", 48, 48, "Eterna"),
            "lime-on-black" =>
                new("/images/brand/mark-lime-on-black.png", 72, 72, "Eterna"),
            "white-on-black" =>
                new("/images/brand/mark-white-on-black.png", 72, 72, "Eterna"),
            "black-on-white" or "light" =>
                new("/images/brand/mark-black-on-white.png", 72, 72, "Eterna"),
            "dark-on-lime" or "lime" =>
                new("/images/brand/mark-dark-on-lime.png", 72, 72, "Eterna"),
            "dark-compact" =>
                new("/images/brand/mark-dark-no-r.png", 48, 48, "Eterna"),
            "lime-slogan-on-dark" or "slogan" =>
                new("/images/brand/mark-lime-slogan-on-dark.png", 220, 140, "Eterna. Building Intelligent Legacies."),
            "lime-slogan-on-black" =>
                new("/images/brand/mark-lime-slogan-on-black.png", 220, 140, "Eterna. Building Intelligent Legacies."),
            "dark-slogan-on-lime" =>
                new("/images/brand/mark-dark-slogan-on-lime.png", 220, 140, "Eterna. Building Intelligent Legacies."),
            "dark-slogan-on-black" =>
                new("/images/brand/mark-dark-slogan-on-black.png", 220, 140, "Eterna. Building Intelligent Legacies."),
            "wordmark-lime-on-dark" or "wordmark" =>
                new("/images/brand/wordmark-lime-on-dark.png", 320, 320, "Eterna"),
            "wordmark-dark-on-lime" =>
                new("/images/brand/wordmark-dark-on-lime.png", 320, 320, "Eterna"),
            _ => new("/images/brand/mark-lime-on-dark.png", 72, 72, "Eterna")
        };
    }
}

public readonly record struct BrandAsset(string Src, int Width, int Height, string Alt);
