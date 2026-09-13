using Eterna.Application.Abstractions;
using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eterna.Infrastructure.Persistence.Repositories;

public sealed class ClientRepository : IClientRepository
{
    private readonly EternaDbContext _dbContext;

    public ClientRepository(EternaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Client>> GetActiveOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clients
            .AsNoTracking()
            .Where(client => client.IsActive)
            .OrderBy(client => client.DisplayOrder)
            .ThenByDescending(client => client.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Client>> GetFeaturedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clients
            .AsNoTracking()
            .Where(client => client.IsActive && client.Featured)
            .OrderBy(client => client.DisplayOrder)
            .ThenByDescending(client => client.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(
                client => client.IsActive && client.Slug == slug,
                cancellationToken);
    }
}
