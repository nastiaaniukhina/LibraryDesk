namespace LibraryDesk.Core.Domain;

/// <summary>РђСЂС…С–РІРЅР° РІРёРґР°С‡Р°: Р»РёС€Рµ РїРѕРєР°Р·СѓС” РґР°РЅС– Р·Р°РІРµСЂС€РµРЅРѕС— РІРёРґР°С‡С– Р№ РЅРµ РјР°С” РјРµС‚РѕРґС–РІ Р·РјС–РЅРё.</summary>
public sealed class ArchivedLoan
{
    private readonly Loan _loan;

    /// <summary>Initializes a new instance of the <see cref="ArchivedLoan"/> class.</summary>
    /// <param name="loan">Р—Р°РІРµСЂС€РµРЅР° РІРёРґР°С‡Р° (Returned Р°Р±Рѕ Cancelled).</param>
    /// <param name="archivedAt">РњРѕРјРµРЅС‚ Р°СЂС…С–РІР°С†С–С—.</param>
    public ArchivedLoan(Loan loan, DateTimeOffset archivedAt)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (loan.Status is not (LoanStatus.Returned or LoanStatus.Cancelled))
        {
            throw new ArgumentException("Р’ Р°СЂС…С–РІ РїРѕС‚СЂР°РїР»СЏСЋС‚СЊ Р»РёС€Рµ Р·Р°РІРµСЂС€РµРЅС– РІРёРґР°С‡С–.", nameof(loan));
        }

        _loan = loan;
        ArchivedAt = archivedAt;
    }

    /// <summary>Gets РјРѕРјРµРЅС‚ Р°СЂС…С–РІР°С†С–С—.</summary>
    public DateTimeOffset ArchivedAt { get; }

    /// <summary>Gets С–РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ РІРёРґР°С‡С–.</summary>
    public int Id => _loan.Id;

    /// <summary>Gets СЃС‚Р°РЅ РІРёРґР°С‡С–.</summary>
    public LoanStatus Status => _loan.Status;

    /// <summary>Gets РїРѕР·РёС†С–С— РІРёРґР°С‡С– Р»РёС€Рµ РґР»СЏ С‡РёС‚Р°РЅРЅСЏ.</summary>
    public IReadOnlyList<LoanItem> Items => _loan.Items;
}
