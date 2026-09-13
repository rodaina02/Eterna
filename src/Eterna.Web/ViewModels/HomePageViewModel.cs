namespace Eterna.Web.ViewModels;

public sealed class HomePageViewModel
{
    public IReadOnlyList<WorkCardViewModel> FeaturedWork { get; init; } = [];
}
