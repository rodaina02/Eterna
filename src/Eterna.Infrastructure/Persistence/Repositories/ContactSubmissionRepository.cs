using Eterna.Application.Abstractions;
using Eterna.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eterna.Infrastructure.Persistence.Repositories;

public sealed class ContactSubmissionRepository : IContactSubmissionRepository
{
    private readonly EternaDbContext _dbContext;

    public ContactSubmissionRepository(EternaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ContactSubmission submission, CancellationToken cancellationToken = default)
    {
        await _dbContext.ContactSubmissions.AddAsync(submission, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEmailDispatchAsync(
        Guid id,
        bool emailSent,
        DateTimeOffset? emailSentAt,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ContactSubmissions
            .Where(submission => submission.Id == id)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(submission => submission.EmailSent, emailSent)
                    .SetProperty(submission => submission.EmailSentAt, emailSentAt),
                cancellationToken);
    }
}
