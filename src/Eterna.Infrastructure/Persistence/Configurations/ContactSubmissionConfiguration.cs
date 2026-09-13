using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eterna.Infrastructure.Persistence.Configurations;

public sealed class ContactSubmissionConfiguration : IEntityTypeConfiguration<ContactSubmission>
{
    public void Configure(EntityTypeBuilder<ContactSubmission> builder)
    {
        builder.ToTable("ContactSubmissions");

        builder.HasKey(submission => submission.Id);

        builder.Property(submission => submission.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(submission => submission.Company)
            .HasMaxLength(160);

        builder.Property(submission => submission.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(submission => submission.Phone)
            .HasMaxLength(40);

        builder.Property(submission => submission.Service)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(submission => submission.Budget)
            .HasMaxLength(40);

        builder.Property(submission => submission.Message)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(submission => submission.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(submission => submission.IpHash)
            .HasMaxLength(64);

        builder.Property(submission => submission.UserAgent)
            .HasMaxLength(256);

        builder.HasIndex(submission => submission.SubmittedAt)
            .HasDatabaseName("IX_ContactSubmissions_SubmittedAt")
            .IsDescending();

        builder.HasIndex(submission => submission.Status)
            .HasDatabaseName("IX_ContactSubmissions_Status");
    }
}
