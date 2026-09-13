using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eterna.Infrastructure.Persistence;

public sealed class EternaDbContext : DbContext
{
    public EternaDbContext(DbContextOptions<EternaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ContactSubmission> ContactSubmissions => Set<ContactSubmission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EternaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
