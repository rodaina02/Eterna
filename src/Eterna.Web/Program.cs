using Eterna.Application;
using Eterna.Application.Options;
using Eterna.Infrastructure;
using Eterna.Infrastructure.Persistence;
using Eterna.Web.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<CompanyOptions>(builder.Configuration.GetSection(CompanyOptions.SectionName));
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection(SiteOptions.SectionName));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));
builder.Services.Configure<RateLimitingOptions>(builder.Configuration.GetSection(RateLimitingOptions.SectionName));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEternaRateLimiting(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery();

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
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/StatusCode", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAntiforgery();
app.MapRazorPages();

app.Run();
