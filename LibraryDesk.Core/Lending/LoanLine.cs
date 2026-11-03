namespace LibraryDesk.Core.Lending;

/// <summary>Рядок формуляра видачі: книжка, кількість примірників і ціна.</summary>
public sealed class LoanLine
{
    /// <summary>Gets or sets ISBN-код книжки.</summary>
    public string Isbn { get; set; } = string.Empty;

    /// <summary>Gets or sets кількість примірників.</summary>
    public int Quantity { get; set; }

    /// <summary>Gets or sets ціну одного примірника, грн.</summary>
    public decimal UnitPrice { get; set; }
}
