namespace LibraryDesk.Core.Domain;

/// <summary>Позиція видачі: книжка, строк у днях і денний тариф.</summary>
public sealed class LoanItem
{
    /// <summary>Код книжки (ISBN).</summary>
    public string Isbn { get; set; }

    /// <summary>Кількість днів видачі.</summary>
    public int Days { get; set; }

    /// <summary>Денний тариф, грн.</summary>
    public decimal DailyRate { get; set; }

    /// <summary>Вартість позиції, грн.</summary>
    public decimal Amount { get; set; }
}
