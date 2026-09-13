using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eterna.Infrastructure.Persistence.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(client => client.Id);

        builder.Property(client => client.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(client => client.Slug)
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(client => client.Industry)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(client => client.ShortDescription)
            .HasMaxLength(280);

        builder.Property(client => client.Description)
            .HasMaxLength(2000);

        builder.Property(client => client.LogoUrl)
            .HasMaxLength(500);

        builder.Property(client => client.HeroImageUrl)
            .HasMaxLength(500);

        builder.Property(client => client.WebsiteUrl)
            .HasMaxLength(300);

        builder.Property(client => client.Services)
            .HasColumnType("text[]")
            .IsRequired();

        builder.Property(client => client.Technologies)
            .HasColumnType("text[]")
            .IsRequired();

        builder.HasIndex(client => client.Slug)
            .IsUnique();

        builder.HasIndex(client => new { client.Featured, client.DisplayOrder })
            .HasDatabaseName("IX_Clients_Featured_DisplayOrder")
            .HasFilter("\"IsActive\" = TRUE");

        builder.HasIndex(client => client.Industry)
            .HasDatabaseName("IX_Clients_Industry")
            .HasFilter("\"IsActive\" = TRUE");
    }
}
