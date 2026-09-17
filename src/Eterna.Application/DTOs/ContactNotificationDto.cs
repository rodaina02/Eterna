namespace Eterna.Application.DTOs;

public sealed record ContactNotificationDto(
    string Name,
    string? Company,
    string Email,
    string? Phone,
    string Service,
    string? Budget,
    string Message,
    DateTimeOffset SubmittedAt);
