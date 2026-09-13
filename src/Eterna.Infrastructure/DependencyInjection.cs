using System.Net.Http.Headers;
using Eterna.Application.Abstractions;
using Eterna.Application.Options;
using Eterna.Infrastructure.Email;
using Eterna.Infrastructure.Persistence;
using Eterna.Infrastructure.Persistence.Repositories;
using Eterna.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eterna.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<EternaDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IContactSubmissionRepository, ContactSubmissionRepository>();
        services.AddSingleton<IClock, SystemClock>();

        services.AddHttpClient<IEmailService, ResendEmailService>((serviceProvider, client) =>
        {
            var emailOptions = serviceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;
            client.BaseAddress = new Uri("https://api.resend.com/");
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(emailOptions.ApiKey))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", emailOptions.ApiKey);
            }
        });

        return services;
    }
}
