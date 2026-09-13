namespace Eterna.Application.Options;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";
    public const string ContactPolicyName = "contact";

    public int ContactPermitLimit { get; set; } = 5;
    public int ContactWindowMinutes { get; set; } = 10;
    public int GlobalPermitLimit { get; set; } = 120;
    public int GlobalWindowMinutes { get; set; } = 1;
}
