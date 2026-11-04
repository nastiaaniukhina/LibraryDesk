namespace LibraryDesk.Core.Domain;

/// <summary>Архівна видача: лише показує дані завершеної видачі й не має методів зміни.</summary>
public sealed class ArchivedLoan
{
    private readonly Loan _loan;

    /// <summary>Initializes a new instance of the <see cref="ArchivedLoan"/> class.</summary>
    /// <param name="loan">Завершена видача (Returned або Cancelled).</param>
    /// <param name="archivedAt">Момент архівації.</param>
    public ArchivedLoan(Loan loan, DateTimeOffset archivedAt)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (loan.Status is not (LoanStatus.Returned or LoanStatus.Cancelled))
        {
            throw new ArgumentException("В архів потрапляють лише завершені видачі.", nameof(loan));
        }

        _loan = loan;
        ArchivedAt = archivedAt;
    }

    /// <summary>Gets момент архівації.</summary>
    public DateTimeOffset ArchivedAt { get; }

    /// <summary>Gets ідентифікатор видачі.</summary>
    public int Id => _loan.Id;

    /// <summary>Gets стан видачі.</summary>
    public LoanStatus Status => _loan.Status;

    /// <summary>Gets позиції видачі лише для читання.</summary>
    public IReadOnlyList<LoanItem> Items => _loan.Items;
}
