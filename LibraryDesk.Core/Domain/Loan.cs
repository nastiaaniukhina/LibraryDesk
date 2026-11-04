namespace LibraryDesk.Core.Domain;

/// <summary>Видача книжок читачеві: агрегат із контрольованими переходами станів.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    /// <summary>Initializes a new instance of the <see cref="Loan"/> class.</summary>
    /// <param name="id">Ідентифікатор видачі, більший за нуль.</param>
    /// <param name="readerId">Ідентифікатор читача, більший за нуль.</param>
    /// <param name="createdAt">Час створення видачі.</param>
    public Loan(int id, int readerId, DateTimeOffset createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(readerId);

        Id = id;
        ReaderId = readerId;
        CreatedAt = createdAt;
        Status = LoanStatus.Reserved;
    }

    /// <summary>Gets ідентифікатор видачі.</summary>
    public int Id { get; }

    /// <summary>Gets ідентифікатор читача.</summary>
    public int ReaderId { get; }

    /// <summary>Gets час створення видачі.</summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>Gets поточний стан видачі.</summary>
    public LoanStatus Status { get; private set; }

    /// <summary>Gets кінцевий строк повернення; null, поки книжки не видано.</summary>
    public DateTimeOffset? DueAt { get; private set; }

    /// <summary>Gets позиції видачі лише для читання.</summary>
    public IReadOnlyList<LoanItem> Items => _items.AsReadOnly();

    /// <summary>Додає позицію до видачі в стані Reserved.</summary>
    /// <param name="item">Позиція видачі.</param>
    public void AddItem(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidOperationException("Позиції додають лише до заброньованої видачі.");
        }

        _items.Add(item);
    }

    /// <summary>Переводить видачу зі стану Reserved у Issued.</summary>
    /// <param name="now">Момент видачі книжок.</param>
    public void Issue(DateTimeOffset now)
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidOperationException("Видати можна лише заброньовану видачу.");
        }

        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Видача не містить жодної книжки.");
        }

        DueAt = now.AddDays(_items.Max(item => item.Days));
        Status = LoanStatus.Issued;
    }

    /// <summary>Переводить видачу зі стану Issued у Overdue, якщо строк минув.</summary>
    /// <param name="now">Поточний момент часу.</param>
    public void MarkOverdue(DateTimeOffset now)
    {
        if (Status != LoanStatus.Issued)
        {
            throw new InvalidOperationException("Прострочити можна лише видану видачу.");
        }

        if (DueAt is not DateTimeOffset due || now <= due)
        {
            throw new InvalidOperationException("Строк повернення ще не минув.");
        }

        Status = LoanStatus.Overdue;
    }

    /// <summary>Повертає видачу зі станів Issued або Overdue.</summary>
    public void Return()
    {
        if (Status is not (LoanStatus.Issued or LoanStatus.Overdue))
        {
            throw new InvalidOperationException("Повернути можна лише видану або прострочену видачу.");
        }

        Status = LoanStatus.Returned;
    }

    /// <summary>Скасовує бронь, поки книжки не видано.</summary>
    public void Cancel()
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidOperationException("Скасувати можна лише заброньовану видачу.");
        }

        Status = LoanStatus.Cancelled;
    }
}
