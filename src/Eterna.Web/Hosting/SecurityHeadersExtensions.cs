using System.Security.Cryptography;

namespace Eterna.Web.Hosting;

public static class SecurityHeadersExtensions
{
    public const string NonceItemKey = "CspNonce";

    public static IApplicationBuilder UseEternaSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            context.Items[NonceItemKey] = nonce;

            var headers = context.Response.Headers;
            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=(), usb=()";

            var environment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var upgrade = environment.IsDevelopment()
                ? string.Empty
                : "; upgrade-insecure-requests";

            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                $"script-src 'self' 'nonce-{nonce}'; " +
                "style-src 'self'; " +
                "img-src 'self' data:; " +
                "font-src 'self'; " +
                "connect-src 'self'; " +
                "object-src 'none'; " +
                "base-uri 'self'; " +
                "form-action 'self'; " +
                "frame-ancestors 'none'; " +
                "frame-src 'none'" +
                upgrade;

            await next();
        });
    }
}
