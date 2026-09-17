using Eterna.Application.Abstractions;
using Eterna.Application.Contacts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Eterna.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ContactFormRequest>, ContactFormValidator>();
        services.AddScoped<IContactSubmissionService, ContactSubmissionService>();
        return services;
    }
}
