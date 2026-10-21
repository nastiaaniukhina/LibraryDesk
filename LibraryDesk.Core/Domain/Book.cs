namespace LibraryDesk.Core.Domain;

/// <summary>Книжка в каталозі бібліотеки.</summary>
public sealed class Book
{
    public string Isbn { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Author { get; init; } = string.Empty;

    public decimal DailyRate { get; init; }

    public int CopiesAvailable { get; init; }
}
