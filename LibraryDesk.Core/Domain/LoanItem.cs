namespace LibraryDesk.Core.Domain;

/// <summary>Незмінна позиція видачі: книжка, строк у днях і денний тариф.</summary>
public sealed record LoanItem
{
    /// <summary>Initializes a new instance of the <see cref="LoanItem"/> class.</summary>
    /// <param name="isbn">ISBN-код книжки, не порожній.</param>
    /// <param name="days">Строк у днях, більший за нуль.</param>
    /// <param name="dailyRate">Денний тариф, не менший за нуль.</param>
    public LoanItem(string isbn, int days, decimal dailyRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(days);
        ArgumentOutOfRangeException.ThrowIfNegative(dailyRate);

        Isbn = isbn;
        Days = days;
        DailyRate = dailyRate;
    }

    /// <summary>Gets ISBN-код книжки.</summary>
    public string Isbn { get; }

    /// <summary>Gets строк видачі в днях.</summary>
    public int Days { get; }

    /// <summary>Gets денний тариф.</summary>
    public decimal DailyRate { get; }

    /// <summary>Gets вартість позиції (обчислюється, а не зберігається).</summary>
    public decimal Amount => Days * DailyRate;
}
