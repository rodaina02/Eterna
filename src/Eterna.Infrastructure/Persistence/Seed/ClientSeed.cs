using Eterna.Application.Abstractions;
using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eterna.Infrastructure.Persistence.Seed;

public static class ClientSeed
{
    public static readonly Guid NabilaHayelId = Guid.Parse("6c1a1a70-4d2b-4f3a-9c11-0f8c0d2a1b10");
    public static readonly Guid CuddsId = Guid.Parse("2e8b4c61-91d3-4a77-8f20-5b1d9e6a3c21");
    public static readonly Guid AtiiqId = Guid.Parse("9a74d0e2-3b58-4c19-a046-7d2f1e8c5a32");
    public static readonly Guid IronGripId = Guid.Parse("4f12c8b9-6e0a-4d85-b173-8a9c2d4e7f43");

    public static async Task EnsureSeededAsync(
        EternaDbContext dbContext,
        IClock clock,
        CancellationToken cancellationToken = default)
    {
        var now = clock.UtcNow;
        var seeds = CreateSeeds(now);

        foreach (var seed in seeds)
        {
            var exists = await dbContext.Clients
                .AnyAsync(client => client.Id == seed.Id || client.Slug == seed.Slug, cancellationToken);

            if (exists)
            {
                continue;
            }

            await dbContext.Clients.AddAsync(seed, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyList<Client> CreateSeeds(DateTimeOffset now)
    {
        return
        [
            new Client
            {
                Id = NabilaHayelId,
                Name = "Nabila Hayel",
                Slug = "nabila-hayel",
                Industry = "Luxury Haute Couture",
                Featured = true,
                DisplayOrder = 1,
                IsActive = true,
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            },
            new Client
            {
                Id = CuddsId,
                Name = "CUDDS",
                Slug = "cudds",
                Industry = "Bedding & Home Essentials",
                Featured = true,
                DisplayOrder = 2,
                IsActive = true,
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
                Services = [],
                Technologies = [],
                CreatedAt = now,
                UpdatedAt = now
            }
        ];
    }
}
