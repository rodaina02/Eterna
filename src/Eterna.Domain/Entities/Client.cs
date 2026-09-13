namespace Eterna.Domain.Entities;

public sealed class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? HeroImageUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string[] Services { get; set; } = [];
    public string[] Technologies { get; set; } = [];
    public bool Featured { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
