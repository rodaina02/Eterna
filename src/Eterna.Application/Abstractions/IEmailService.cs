using Eterna.Application.DTOs;

namespace Eterna.Application.Abstractions;

public interface IEmailService
{
    Task<EmailSendResult> SendNotificationAsync(
        ContactNotificationDto notification,
        CancellationToken cancellationToken = default);

    Task<EmailSendResult> SendConfirmationAsync(
        ContactConfirmationDto confirmation,
        CancellationToken cancellationToken = default);
}
