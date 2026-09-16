using Eterna.Application.Abstractions;
using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eterna.Infrastructure.Persistence.Seed;

public static class ClientSeed
{
    public static readonly Guid CuddsId = Guid.Parse("2e8b4c61-91d3-4a77-8f20-5b1d9e6a3c21");
    public static readonly Guid NabilaHayelId = Guid.Parse("6c1a1a70-4d2b-4f3a-9c11-0f8c0d2a1b10");
    public static readonly Guid AtiiqId = Guid.Parse("9a74d0e2-3b58-4c19-a046-7d2f1e8c5a32");
    public static readonly Guid IronGripId = Guid.Parse("4f12c8b9-6e0a-4d85-b173-8a9c2d4e7f43");
    public static readonly Guid ReehanBahaaId = Guid.Parse("7b3e9a14-2c6f-4d81-9e05-1a8f4c2d6b57");
    public static readonly Guid AfterTaleId = Guid.Parse("a1c5e8d3-7f20-4b46-8d19-3e6a9c1b5f08");

    public static async Task EnsureSeededAsync(
        EternaDbContext dbContext,
        IClock clock,
        CancellationToken cancellationToken = default)
    {
        var now = clock.UtcNow;
        var seeds = CreateSeeds(now);

        foreach (var seed in seeds)
        {
            var existing = await dbContext.Clients
                .FirstOrDefaultAsync(
                    client => client.Id == seed.Id || client.Slug == seed.Slug,
                    cancellationToken);

            if (existing is null)
            {
                await dbContext.Clients.AddAsync(seed, cancellationToken);
                continue;
            }

            existing.Name = seed.Name;
            existing.Industry = seed.Industry;
            existing.LogoUrl = seed.LogoUrl;
            existing.WebsiteUrl = seed.WebsiteUrl;
            existing.Featured = seed.Featured;
            existing.DisplayOrder = seed.DisplayOrder;
            existing.IsActive = true;
            existing.UpdatedAt = now;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyList<Client> CreateSeeds(DateTimeOffset now)
    {
        return
        [
            new Client
            {
                Id = CuddsId,
                Name = "CUDDS",
                Slug = "cudds",
                Industry = "Bedding & Home Essentials",
                Featured = true,
                DisplayOrder = 1,
                IsActive = true,
                LogoUrl = "/images/clients/cudds-light.png",
                WebsiteUrl = "http://cudds.shop/",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = NabilaHayelId,
                Name = "Nabila Hayel",
                Slug = "nabila-hayel",
                Industry = "Luxury Haute Couture",
                Featured = true,
                DisplayOrder = 2,
                IsActive = true,
                LogoUrl = "/images/clients/nabila-hayel-gold-transparent.png",
                WebsiteUrl = "https://nabilahayel.com/",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = AtiiqId,
                Name = "ATIIQ",
                Slug = "atiiq",
                Industry = "Handmade Crafts",
                Featured = true,
                DisplayOrder = 3,
                IsActive = true,
                LogoUrl = "/images/clients/atiiq-light.png",
                WebsiteUrl = "https://atiiq.com/",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = IronGripId,
                Name = "IRON GRIP",
                Slug = "iron-grip",
                Industry = "Sports & Athleisure",
                Featured = true,
                DisplayOrder = 4,
                IsActive = true,
                LogoUrl = "/images/clients/iron-grip-light.png",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = ReehanBahaaId,
                Name = "Reehan Bahaa",
                Slug = "reehan-bahaa",
                Industry = "Fashion",
                Featured = true,
                DisplayOrder = 5,
                IsActive = true,
                WebsiteUrl = "https://reehanbahaa.com/",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = AfterTaleId,
                Name = "AfterTale",
                Slug = "aftertale",
                Industry = "Business development & strategy",
                Featured = true,
                DisplayOrder = 6,
                IsActive = true,
                LogoUrl = "/images/clients/aftertale-wordmark.png",
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            }
        ];
    }
}
