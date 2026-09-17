namespace Eterna.Application.Contacts;

public sealed record ContactSubmitContext(
    string? RemoteIpAddress,
    string? UserAgent);
