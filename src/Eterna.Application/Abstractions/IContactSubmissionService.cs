using Eterna.Application.Contacts;

namespace Eterna.Application.Abstractions;

public interface IContactSubmissionService
{
    Task<ContactSubmitResult> SubmitAsync(
        ContactFormRequest request,
        ContactSubmitContext context,
        CancellationToken cancellationToken = default);
}
