namespace Eterna.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
