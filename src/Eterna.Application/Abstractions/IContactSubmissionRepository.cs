using Eterna.Domain.Entities;

namespace Eterna.Application.Abstractions;

public interface IContactSubmissionRepository
{
    Task AddAsync(ContactSubmission submission, CancellationToken cancellationToken = default);
    Task UpdateEmailDispatchAsync(
        Guid id,
        bool emailSent,
        DateTimeOffset? emailSentAt,
        CancellationToken cancellationToken = default);
}
