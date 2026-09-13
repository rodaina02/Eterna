namespace Eterna.Web.ViewModels;

public sealed class ProcessStageViewModel
{
    public string Index { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public bool IsContinuation { get; init; }
}
