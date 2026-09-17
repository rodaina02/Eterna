using Eterna.Application;
using Eterna.Application.Options;
using Eterna.Infrastructure;
using Eterna.Infrastructure.Persistence;
using Eterna.Web.Hosting;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<CompanyOptions>(builder.Configuration.GetSection(CompanyOptions.SectionName));
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection(SiteOptions.SectionName));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));
builder.Services.Configure<ContactOptions>(builder.Configuration.GetSection(ContactOptions.SectionName));
builder.Services.Configure<RateLimitingOptions>(builder.Configuration.GetSection(RateLimitingOptions.SectionName));
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueCountLimit = 32;
    options.ValueLengthLimit = 8 * 1024;
    options.MultipartBodyLengthLimit = 64 * 1024;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEternaRateLimiting(builder.Configuration);
builder.Services.AddRazorPages(options =>
{
    options.Conventions.Add(new ContactRateLimitConvention());
});
builder.Services.AddAntiforgery();
builder.Services.AddHealthChecks();

var app = builder.Build();

try
{
    await DatabaseInitializer.InitializeAsync(app.Services);
}
catch (Exception exception)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Eterna.Startup");
    logger.LogError(exception, "Database initialization failed. The site will start without a live database.");
}

if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseEternaSecurityHeaders();
app.UseStatusCodePagesWithReExecute("/StatusCode", "?code={0}");
app.UseHttpsRedirection();

var cacheStaticAssets = !app.Environment.IsDevelopment();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = staticFileContext =>
    {
        if (cacheStaticAssets)
        {
            staticFileContext.Context.Response.Headers.CacheControl = "public,max-age=604800";
        }
    }
});
app.UseRouting();
app.UseRateLimiter();
app.UseAntiforgery();
app.MapHealthChecks("/health").DisableRateLimiting();
app.MapRazorPages();

app.Run();

public partial class Program
{
}
