namespace LibraryDesk.Core.Domain;

/// <summary>Видача книжок читачеві.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    /// <summary>Номер видачі.</summary>
    public int Id { get; set; }

    /// <summary>Номер читача.</summary>
    public int ReaderId { get; set; }

    /// <summary>Поточний стан видачі.</summary>
    public LoanStatus Status { get; set; }

    /// <summary>Дата й час створення видачі.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Позиції видачі (лише для читання).</summary>
    public IReadOnlyList<LoanItem> Items { get; }

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
