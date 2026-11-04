using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>Сховище видач у пам'яті на основі словника.</summary>
public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly Dictionary<int, Loan> _loans = new();

    /// <inheritdoc/>
    public void Add(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (!_loans.TryAdd(loan.Id, loan))
        {
            throw new InvalidOperationException("Видача з таким Id уже існує.");
        }
    }

    /// <inheritdoc/>
    public Loan? GetById(int id) => _loans.GetValueOrDefault(id);

    /// <inheritdoc/>
    public IReadOnlyList<Loan> GetAll() => _loans.Values.ToList();
}
