using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>РЎС…РѕРІРёС‰Рµ РІРёРґР°С‡ Сѓ РїР°Рј'СЏС‚С– РЅР° РѕСЃРЅРѕРІС– СЃР»РѕРІРЅРёРєР°.</summary>
public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly Dictionary<int, Loan> _loans = new();

    /// <inheritdoc/>
    public void Add(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (!_loans.TryAdd(loan.Id, loan))
        {
            throw new InvalidOperationException("Р’РёРґР°С‡Р° Р· С‚Р°РєРёРј Id СѓР¶Рµ С–СЃРЅСѓС”.");
        }
    }

    /// <inheritdoc/>
    public Loan? GetById(int id) => _loans.GetValueOrDefault(id);

    /// <inheritdoc/>
    public IReadOnlyList<Loan> GetAll() => _loans.Values.ToList();

    /// <inheritdoc/>
    public void Save()
    {
    }
}
