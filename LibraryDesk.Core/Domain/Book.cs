namespace LibraryDesk.Core.Domain;

/// <summary>Книжка в каталозі бібліотеки.</summary>
public sealed class Book
{
    /// <summary>Gets the book ISBN.</summary>
    public string Isbn { get; init; } = string.Empty;

    /// <summary>Gets the book title.</summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>Gets the book author.</summary>
    public string Author { get; init; } = string.Empty;

    /// <summary>Gets the daily usage rate in UAH.</summary>
    public decimal DailyRate { get; init; }
}
