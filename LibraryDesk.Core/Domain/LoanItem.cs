namespace LibraryDesk.Core.Domain;

/// <summary>Позиція видачі: книжка, строк у днях і денний тариф.</summary>
public sealed class LoanItem
{
    public string Isbn { get; init; } = string.Empty;

    public int Days { get; init; }

    public decimal DailyRate { get; init; }

    public decimal Amount => Days * DailyRate;
}