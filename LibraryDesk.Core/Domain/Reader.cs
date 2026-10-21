namespace LibraryDesk.Core.Domain;

/// <summary>Читач бібліотеки.</summary>
public sealed class Reader
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public bool IsStudent { get; init; }
}