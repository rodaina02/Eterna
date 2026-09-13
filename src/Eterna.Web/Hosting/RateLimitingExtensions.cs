using System.Threading.RateLimiting;
using Eterna.Application.Options;
using Microsoft.AspNetCore.RateLimiting;

namespace Eterna.Web.Hosting;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddEternaRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(RateLimitingOptions.SectionName)
            .Get<RateLimitingOptions>() ?? new RateLimitingOptions();

        services.AddRateLimiter(limiter =>
        {
            limiter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiter.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = options.GlobalPermitLimit,
                        Window = TimeSpan.FromMinutes(options.GlobalWindowMinutes),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });

            limiter.AddFixedWindowLimiter(RateLimitingOptions.ContactPolicyName, window =>
            {
                window.PermitLimit = options.ContactPermitLimit;
                window.Window = TimeSpan.FromMinutes(options.ContactWindowMinutes);
                window.QueueLimit = 0;
                window.AutoReplenishment = true;
            });
        });

        return services;
    }
}
