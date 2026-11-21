namespace LibraryDesk.Core.Domain;

/// <summary>Видача книжок читачеві: агрегат із контрольованими переходами станів.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    /// <summary>Initializes a new instance of the <see cref="Loan"/> class.</summary>
    /// <param name="id">Ідентифікатор видачі, більший за нуль.</param>
    /// <param name="readerId">Ідентифікатор читача, більший за нуль.</param>
    /// <param name="createdAt">Час створення видачі.</param>
    public Loan(int id, int readerId, DateTimeOffset createdAt, ReaderKind readerKind = ReaderKind.Regular)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(readerId);

        Id = id;
        ReaderId = readerId;
        CreatedAt = createdAt;
        ReaderKind = readerKind;
        Status = LoanStatus.Reserved;
    }

    /// <summary>Gets ідентифікатор видачі.</summary>
    public int Id { get; }

    /// <summary>Gets ідентифікатор читача.</summary>
    public int ReaderId { get; }

    /// <summary>Gets час створення видачі.</summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>Gets категорію читача.</summary>
    public ReaderKind ReaderKind { get; private set; }

    /// <summary>Gets поточний стан видачі.</summary>
    public LoanStatus Status { get; private set; }

    /// <summary>Gets кінцевий строк повернення; null, поки книжки не видано.</summary>
    public DateTimeOffset? DueAt { get; private set; }

    /// <summary>Gets момент повернення; null, поки книжки не повернуто.</summary>
    public DateTimeOffset? ReturnedAt { get; private set; }

    /// <summary>Gets позиції видачі лише для читання.</summary>
    public IReadOnlyList<LoanItem> Items => _items.AsReadOnly();

    /// <summary>Додає позицію до видачі в стані Reserved.</summary>
    /// <param name="item">Позиція видачі.</param>
    public void AddItem(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException(
                "Додавати книжки можна лише до заброньованої видачі.");
        }

        _items.Add(item);
    }

    /// <summary>Переводить видачу зі стану Reserved у Issued.</summary>
    /// <param name="now">Момент видачі книжок.</param>
    public void Issue(DateTimeOffset now)
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException("Видати можна лише заброньовану видачу.");
        }

        if (_items.Count == 0)
        {
            throw new InvalidLoanStateException("Видача не містить жодної книжки.");
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
            throw new InvalidLoanStateException("Прострочити можна лише видану видачу.");
        }

        if (DueAt is not DateTimeOffset due || now < due)
        {
            throw new InvalidLoanStateException("Строк повернення ще не минув.");
        }

        Status = LoanStatus.Overdue;
    }

    /// <summary>Обчислює кількість календарних днів прострочення на вказану дату.</summary>
    /// <param name="asOf">Дата, на яку обчислюється прострочення.</param>
    /// <returns>Кількість днів прострочення, але не менше нуля.</returns>
    public int OverdueDaysAt(DateOnly asOf)
    {
        if (DueAt is not DateTimeOffset dueAt)
        {
            return 0;
        }

        DateOnly dueDate = DateOnly.FromDateTime(dueAt.DateTime);
        return Math.Max(asOf.DayNumber - dueDate.DayNumber, 0);
    }

    /// <summary>Повертає видачу зі станів Issued або Overdue.</summary>
    /// <param name="now">Момент повернення книжок.</param>
    public void Return(DateTimeOffset now)
    {
        if (Status is not (LoanStatus.Issued or LoanStatus.Overdue))
        {
            throw new InvalidLoanStateException(
                "Повернути можна лише видану або прострочену видачу.");
        }

        ReturnedAt = now;
        Status = LoanStatus.Returned;
    }

    /// <summary>Відновлює видачу зі збереженого запису.</summary>
    /// <param name="id">Ідентифікатор видачі.</param>
    /// <param name="readerId">Ідентифікатор читача.</param>
    /// <param name="createdAt">Час створення.</param>
    /// <param name="kind">Категорія читача.</param>
    /// <param name="status">Збережений стан.</param>
    /// <param name="dueAt">Строк повернення.</param>
    /// <param name="returnedAt">Час повернення.</param>
    /// <param name="items">Позиції видачі.</param>
    /// <returns>Відновлена видача.</returns>
    internal static Loan Restore(
        int id,
        int readerId,
        DateTimeOffset createdAt,
        ReaderKind kind,
        LoanStatus status,
        DateTimeOffset? dueAt,
        DateTimeOffset? returnedAt,
        IEnumerable<LoanItem> items)
    {
        Loan loan = new(id, readerId, createdAt, kind);
        foreach (LoanItem item in items)
        {
            loan._items.Add(item);
        }

        loan.Status = status;
        loan.DueAt = dueAt;
        loan.ReturnedAt = returnedAt;
        return loan;
    }

    /// <summary>Скасовує бронь, поки книжки не видано.</summary>
    public void Cancel()
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException("Скасувати можна лише заброньовану видачу.");
        }

        Status = LoanStatus.Cancelled;
    }
}
