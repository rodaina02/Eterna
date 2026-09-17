using Eterna.Application.Abstractions;
using Eterna.Web.Content;
using Eterna.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eterna.Web.Pages;

public sealed class IndexModel : PageModel
{
    private readonly IClientRepository _clients;

    public IndexModel(IClientRepository clients)
    {
        _clients = clients;
    }

    public HomePageViewModel Home { get; private set; } = new();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Eterna — Software, AI & Intelligent Systems";
        ViewData["Description"] =
            "Eterna is a software and AI solutions company building durable digital products and intelligent systems for modern enterprises.";
        ViewData["CanonicalPath"] = "/";
        ViewData["HeaderTheme"] = "black";
        ViewData["BodyTheme"] = "black";
        ViewData["MainClass"] = "site-main--home";

        Home = new HomePageViewModel
        {
            FeaturedWork = await LoadFeaturedWorkAsync(cancellationToken)
        };
    }

    private async Task<IReadOnlyList<WorkCardViewModel>> LoadFeaturedWorkAsync(CancellationToken cancellationToken)
    {
        try
        {
            var clients = await _clients.GetFeaturedAsync(cancellationToken);
            if (clients.Count == 0)
            {
                return HomeContent.KnownWork;
            }

            var bySlug = clients.ToDictionary(client => client.Slug, StringComparer.OrdinalIgnoreCase);

            return WorkContent.Clients
                .Select((project, index) =>
                {
                    bySlug.TryGetValue(project.Id, out var live);
                    var card = WorkContent.ToWorkCard(project, index);
                    if (live is null)
                    {
                        return card;
                    }

                    return new WorkCardViewModel
                    {
                        Client = live.Name,
                        Industry = live.Industry,
                        Label = card.Label,
                        Index = card.Index,
                        Href = card.Href,
                        Modifier = card.Modifier,
                        LogoSrc = card.LogoSrc,
                        LogoWidth = card.LogoWidth,
                        LogoHeight = card.LogoHeight
                    };
                })
                .ToList();
        }
        catch
        {
            return HomeContent.KnownWork;
        }
    }
}
