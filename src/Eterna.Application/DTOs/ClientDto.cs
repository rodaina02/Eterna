namespace Eterna.Application.DTOs;

public sealed record ClientDto(
    Guid Id,
    string Name,
    string Slug,
    string Industry,
    string? ShortDescription,
    string? Description,
    string? LogoUrl,
    string? HeroImageUrl,
    string? WebsiteUrl,
    IReadOnlyList<string> Services,
    IReadOnlyList<string> Technologies,
    bool Featured,
    int DisplayOrder);
