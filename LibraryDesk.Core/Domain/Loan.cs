namespace LibraryDesk.Core.Domain;

/// <summary>Видача книжок читачеві.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    /// <summary>Gets or sets the loan identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the reader identifier.</summary>
    public int ReaderId { get; set; }

    /// <summary>Gets or sets the current loan status.</summary>
    public LoanStatus Status { get; set; }

    /// <summary>Gets or sets the loan creation date and time.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets the loan items.</summary>
    public IReadOnlyList<LoanItem> Items => _items;

    /// <summary>Додає позицію до видачі.</summary>
    /// <param name="item">Позиція видачі, яку потрібно додати.</param>
    public void AddItem(LoanItem item) => _items.Add(item);

    /// <summary>Обчислює підсумкову вартість видачі.</summary>
    /// <returns>Сума вартості всіх позицій видачі.</returns>
    public decimal Total()
    {
        decimal sum = 0m;
        foreach (LoanItem item in _items)
        {
            sum += item.Amount;
        }

        return sum;
    }
}
