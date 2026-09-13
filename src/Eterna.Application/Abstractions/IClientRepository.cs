using Eterna.Domain.Entities;

namespace Eterna.Application.Abstractions;

public interface IClientRepository
{
    Task<IReadOnlyList<Client>> GetActiveOrderedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Client>> GetFeaturedAsync(CancellationToken cancellationToken = default);
    Task<Client?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
