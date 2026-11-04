using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Services;

/// <summary>Прикладний сервіс видач: сценарій і розрахунок суми.</summary>
public sealed class LoanService
{
    private readonly ILoanRepository _repository;
    private readonly IFeePolicy _feePolicy;
    private readonly INotifier _notifier;

    /// <summary>Initializes a new instance of the <see cref="LoanService"/> class.</summary>
    /// <param name="repository">Сховище видач.</param>
    /// <param name="feePolicy">Політика розрахунку плати.</param>
    /// <param name="notifier">Сповіщувач.</param>
    public LoanService(ILoanRepository repository, IFeePolicy feePolicy, INotifier notifier)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(feePolicy);
        ArgumentNullException.ThrowIfNull(notifier);

        _repository = repository;
        _feePolicy = feePolicy;
        _notifier = notifier;
    }

    /// <summary>Реєструє видачу й повідомляє читача.</summary>
    /// <param name="loan">Видача.</param>
    /// <param name="reader">Читач, якому належить видача.</param>
    public void Place(Loan loan, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentNullException.ThrowIfNull(reader);
        if (loan.ReaderId != reader.Id)
        {
            throw new ArgumentException("Видача належить іншому читачеві.", nameof(reader));
        }

        _repository.Add(loan);
        _notifier.Notify(reader.Email, "Видачу оформлено.");
    }

    /// <summary>Рахує суму видачі за поточною політикою.</summary>
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
}
