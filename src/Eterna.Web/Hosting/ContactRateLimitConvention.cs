using Eterna.Application.Options;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.RateLimiting;

namespace Eterna.Web.Hosting;

public sealed class ContactRateLimitConvention : IPageApplicationModelConvention
{
    public void Apply(PageApplicationModel model)
    {
        if (!string.Equals(model.ViewEnginePath, "/Contact/Index", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        model.EndpointMetadata.Add(new EnableRateLimitingAttribute(RateLimitingOptions.ContactPolicyName));
    }
}
