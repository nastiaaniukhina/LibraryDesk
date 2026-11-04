namespace LibraryDesk.Core.Domain;

/// <summary>Книжка в каталозі: змінюється лише кількість вільних примірників.</summary>
public sealed class Book
{
    /// <summary>Initializes a new instance of the <see cref="Book"/> class.</summary>
    /// <param name="isbn">ISBN-код, не порожній.</param>
    /// <param name="title">Назва, не порожня.</param>
    /// <param name="author">Автор, не порожній.</param>
    /// <param name="dailyRate">Денний тариф, не менший за нуль.</param>
    /// <param name="copiesAvailable">Кількість вільних примірників, не менша за нуль.</param>
    public Book(string isbn, string title, string author, decimal dailyRate, int copiesAvailable)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(author);
        ArgumentOutOfRangeException.ThrowIfNegative(dailyRate);
        ArgumentOutOfRangeException.ThrowIfNegative(copiesAvailable);

        Isbn = isbn;
        Title = title;
        Author = author;
        DailyRate = dailyRate;
        CopiesAvailable = copiesAvailable;
    }

    /// <summary>Gets ISBN-код книжки.</summary>
    public string Isbn { get; }

    /// <summary>Gets назву книжки.</summary>
    public string Title { get; }

    /// <summary>Gets автора книжки.</summary>
    public string Author { get; }

    /// <summary>Gets денний тариф.</summary>
    public decimal DailyRate { get; }

    /// <summary>Gets кількість вільних примірників.</summary>
    public int CopiesAvailable { get; private set; }

    /// <summary>Забирає один примірник зі сховища.</summary>
    public void TakeCopy()
    {
        if (CopiesAvailable == 0)
        {
            throw new InvalidOperationException("Вільних примірників немає.");
        }

        CopiesAvailable--;
    }

    /// <summary>Повертає один примірник до сховища.</summary>
    public void ReturnCopy()
    {
        CopiesAvailable++;
    }
}
