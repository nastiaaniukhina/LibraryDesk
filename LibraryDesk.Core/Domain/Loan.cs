namespace LibraryDesk.Core.Domain;

/// <summary>Видача книжок читачеві.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    public int Id { get; init; }

    public int ReaderId { get; init; }

    public LoanStatus Status { get; set; } = LoanStatus.Reserved;

    public DateTimeOffset CreatedAt { get; init; }

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
