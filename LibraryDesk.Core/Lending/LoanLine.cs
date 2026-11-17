namespace LibraryDesk.Core.Lending;

/// <summary>Рядок формуляра видачі: книжка, кількість примірників і ціна.</summary>
public sealed class LoanLine
{
    /// <summary>Gets iSBN-код книжки.</summary>
    public string Isbn { get; init; } = string.Empty;

    /// <summary>Gets кількість примірників.</summary>
    public int Quantity { get; init; }

    /// <summary>Gets ціну одного примірника, грн.</summary>
    public decimal UnitPrice { get; init; }
}
