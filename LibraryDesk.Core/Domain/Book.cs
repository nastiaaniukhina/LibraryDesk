namespace LibraryDesk.Core.Domain;

/// <summary>Книжка в каталозі бібліотеки.</summary>
public sealed class Book
{
    /// <summary>Міжнародний номер книжки (ISBN).</summary>
    public string Isbn { get; init; } = string.Empty;

    /// <summary>Назва книжки.</summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>Автор книжки.</summary>
    public string Author { get; init; } = string.Empty;

    /// <summary>Денний тариф користування, грн.</summary>
    public decimal DailyRate { get; init; }  

}
