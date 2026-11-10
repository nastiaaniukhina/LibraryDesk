using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Errors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryDesk.Core.Services;

/// <summary>
/// Прикладний сервіс видач: сценарій і розрахунок суми.
/// </summary>
public sealed class LoanService
{
    private static readonly Action<ILogger, int, Exception?> StorageFailure =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(1, nameof(StorageFailure)),
            "Збій сховища, {LoanId}");

    private static readonly Func<ILogger, int, IDisposable?> LoanScope =
        LoggerMessage.DefineScope<int>("Loan:{LoanId}");

    private static readonly Action<ILogger, int, int, Exception?> PlacementStarted =
        LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(2, nameof(PlacementStarted)),
            "Початок оформлення, читач {ReaderId}, позицій {ItemCount}");

    private static readonly Action<ILogger, int, Exception?> EmptyLoanWarning =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(3, nameof(EmptyLoanWarning)),
            "Видача {LoanId} не містить жодної позиції");

    private static readonly Action<ILogger, int, Exception?> NotificationWarning =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(4, nameof(NotificationWarning)),
            "Сповіщення читачу {ReaderId} не надіслано");

    private static readonly Action<ILogger, int, Exception?> LoanPlaced =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(5, nameof(LoanPlaced)),
            "Видачу оформлено, читач {ReaderId}");

    private static readonly Action<ILogger, Exception?> BooksReturned =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(6, nameof(BooksReturned)),
            "Книжки повернено");

    private readonly ILoanRepository _repository;
    private readonly IFeePolicy _feePolicy;
    private readonly INotifier _notifier;
    private readonly ILogger<LoanService> _log;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoanService"/> class.
    /// </summary>
    /// <param name="repository">Сховище видач.</param>
    /// <param name="feePolicy">Політика розрахунку плати.</param>
    /// <param name="notifier">Сповіщувач.</param>
    /// <param name="logger">Логер; якщо не передано, журнал не ведеться.</param>
    public LoanService(
        ILoanRepository repository,
        IFeePolicy feePolicy,
        INotifier notifier,
        ILogger<LoanService>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(feePolicy);
        ArgumentNullException.ThrowIfNull(notifier);

        _repository = repository;
        _feePolicy = feePolicy;
        _notifier = notifier;
        _log = logger ?? NullLogger<LoanService>.Instance;
    }

    /// <summary>
    /// Рахує коректні значення днів.
    /// </summary>
    /// <param name="rows">Рядки з числами.</param>
    /// <returns>Кількість додатних чисел.</returns>
    public static int CountValidDays(string[] rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        int valid = 0;

        foreach (string row in rows)
        {
            if (int.TryParse(row, out int days) && days > 0)
            {
                valid++;
            }
        }

        return valid;
    }

    /// <summary>
    /// Реєструє видачу й повідомляє читача.
    /// </summary>
    /// <param name="loan">Видача.</param>
    /// <param name="reader">Читач, якому належить видача.</param>
    public void Place(Loan loan, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentNullException.ThrowIfNull(reader);

        if (loan.ReaderId != reader.Id)
        {
            throw new ArgumentException(
                "Видача належить іншому читачеві.",
                nameof(reader));
        }

        using IDisposable? scope = LoanScope(_log, loan.Id);

        PlacementStarted(_log, reader.Id, loan.Items.Count, null);

        if (loan.Items.Count == 0)
        {
            EmptyLoanWarning(_log, loan.Id, null);
        }

        _repository.Add(loan);

        try
        {
            _notifier.Notify(reader.Email, "Видачу оформлено.");
        }
        catch (IOException ex)
        {
            NotificationWarning(_log, reader.Id, ex);
        }

        LoanPlaced(_log, reader.Id, null);
    }

    /// <summary>
    /// Рахує суму видачі за поточною політикою.
    /// </summary>
    /// <param name="loan">Видача.</param>
    /// <returns>Сума плати за всі позиції.</returns>
    public decimal TotalOf(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        decimal total = 0m;

        foreach (LoanItem item in loan.Items)
        {
            total += _feePolicy.PriceOf(item);
        }

        return total;
    }

    /// <summary>
    /// Додає типізовану позицію до заброньованої видачі.
    /// </summary>
    /// <param name="loanId">Ідентифікатор видачі.</param>
    /// <param name="item">Позиція видачі.</param>
    public void AddItem(int loanId, LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Loan loan = _repository.GetById(loanId)
            ?? throw new DomainRuleException(
                "loan.exists",
                $"Видачу {loanId} не знайдено");

        if (loan.Status != LoanStatus.Reserved)
        {
            throw new DomainRuleException(
                "loan.editable",
                $"Стан {loan.Status} не дозволяє додавати позиції");
        }

        loan.AddItem(item);
    }

    /// <summary>
    /// Повертає видану або прострочену видачу.
    /// </summary>
    /// <param name="loanId">Ідентифікатор видачі.</param>
    public void ReturnLoan(int loanId)
    {
        using IDisposable? scope = LoanScope(_log, loanId);

        Loan loan = _repository.GetById(loanId)
            ?? throw new DomainRuleException(
                "loan.exists",
                $"Видачу {loanId} не знайдено");

        if (loan.Status is not (LoanStatus.Issued or LoanStatus.Overdue))
        {
            throw new DomainRuleException(
                "loan.returnable",
                $"Стан {loan.Status} не дозволяє повернення");
        }

        loan.Return();

        BooksReturned(_log, null);
    }

    /// <summary>
    /// Завантажує видачу.
    /// </summary>
    /// <param name="loanId">Ідентифікатор видачі.</param>
    /// <returns>Видача.</returns>
    public Loan Load(int loanId) =>
        _repository.GetById(loanId)
            ?? throw new DomainRuleException(
                "loan.exists",
                $"Видачу {loanId} не знайдено");

    /// <summary>
    /// Скасовує видачу.
    /// </summary>
    /// <param name="loanId">Ідентифікатор видачі.</param>
    public void Cancel(int loanId)
    {
        try
        {
            Loan loan = _repository.GetById(loanId)
                ?? throw new DomainRuleException(
                    "loan.exists",
                    $"Видачу {loanId} не знайдено");

            loan.Cancel();
        }
        catch (IOException ex)
        {
            StorageFailure(_log, loanId, ex);
            throw;
        }
    }
}
