using System.Threading.RateLimiting;
using Eterna.Application.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

            limiter.OnRejected = (context, _) =>
            {
                var http = context.HttpContext;
                if (HttpMethods.IsPost(http.Request.Method)
                    && http.Request.Path.StartsWithSegments("/contact"))
                {
                    var tempData = http.RequestServices
                        .GetRequiredService<ITempDataDictionaryFactory>()
                        .GetTempData(http);
                    tempData[ContactFormStatuses.TempDataKey] = ContactFormStatuses.RateLimited;
                    tempData.Save();
                    http.Response.Redirect("/contact");
                    return ValueTask.CompletedTask;
                }

                http.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            };

            limiter.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var partitionKey = PartitionKey(context);

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

            limiter.AddPolicy(RateLimitingOptions.ContactPolicyName, context =>
            {
                if (!HttpMethods.IsPost(context.Request.Method))
                {
                    return RateLimitPartition.GetNoLimiter("contact-read");
                }

                var partitionKey = PartitionKey(context);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = options.ContactPermitLimit,
                        Window = TimeSpan.FromMinutes(options.ContactWindowMinutes),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });
        });

        return services;
    }

    private static string PartitionKey(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
