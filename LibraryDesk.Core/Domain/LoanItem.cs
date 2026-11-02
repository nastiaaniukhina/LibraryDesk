namespace LibraryDesk.Core.Domain;

/// <summary>Позиція видачі: книжка, строк у днях і денний тариф.</summary>
public sealed class LoanItem
{
    /// <summary>Gets or sets the book ISBN.</summary>
    public string Isbn { get; set; } = string.Empty;

    /// <summary>Gets or sets the loan duration in days.</summary>
    public int Days { get; set; }

    /// <summary>Gets or sets the daily rate in UAH.</summary>
    public decimal DailyRate { get; set; }

    /// <summary>Gets or sets the item amount in UAH.</summary>
    public decimal Amount { get; set; }
}
