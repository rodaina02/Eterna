namespace Eterna.Application.DTOs;

public sealed record EmailSendResult(bool Succeeded, string? Error = null);
