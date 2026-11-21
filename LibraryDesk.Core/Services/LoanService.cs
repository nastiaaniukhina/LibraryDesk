using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Services;

/// <summary>РџСЂРёРєР»Р°РґРЅРёР№ СЃРµСЂРІС–СЃ РІРёРґР°С‡: СЃС†РµРЅР°СЂС–Р№ С– СЂРѕР·СЂР°С…СѓРЅРѕРє СЃСѓРјРё.</summary>
public sealed class LoanService
{
    private readonly ILoanRepository _repository;
    private readonly IFeePolicy _feePolicy;
    private readonly INotifier _notifier;

    /// <summary>Initializes a new instance of the <see cref="LoanService"/> class.</summary>
    /// <param name="repository">РЎС…РѕРІРёС‰Рµ РІРёРґР°С‡.</param>
    /// <param name="feePolicy">РџРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РїР»Р°С‚Рё.</param>
    /// <param name="notifier">РЎРїРѕРІС–С‰СѓРІР°С‡.</param>
    public LoanService(ILoanRepository repository, IFeePolicy feePolicy, INotifier notifier)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(feePolicy);
        ArgumentNullException.ThrowIfNull(notifier);

        _repository = repository;
        _feePolicy = feePolicy;
        _notifier = notifier;
    }

    /// <summary>Р РµС”СЃС‚СЂСѓС” РІРёРґР°С‡Сѓ Р№ РїРѕРІС–РґРѕРјР»СЏС” С‡РёС‚Р°С‡Р°.</summary>
    /// <param name="loan">Р’РёРґР°С‡Р°.</param>
    /// <param name="reader">Р§РёС‚Р°С‡, СЏРєРѕРјСѓ РЅР°Р»РµР¶РёС‚СЊ РІРёРґР°С‡Р°.</param>
    public void Place(Loan loan, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentNullException.ThrowIfNull(reader);
        if (loan.ReaderId != reader.Id)
        {
            throw new ArgumentException("Р’РёРґР°С‡Р° РЅР°Р»РµР¶РёС‚СЊ С–РЅС€РѕРјСѓ С‡РёС‚Р°С‡РµРІС–.", nameof(reader));
        }

        _repository.Add(loan);
        _notifier.Notify(reader.Email, "Р’РёРґР°С‡Сѓ РѕС„РѕСЂРјР»РµРЅРѕ.");
    }

    /// <summary>Р Р°С…СѓС” СЃСѓРјСѓ РІРёРґР°С‡С– Р·Р° РїРѕС‚РѕС‡РЅРѕСЋ РїРѕР»С–С‚РёРєРѕСЋ.</summary>
    /// <param name="loan">Р’РёРґР°С‡Р°.</param>
    /// <returns>РЎСѓРјР° РїР»Р°С‚Рё Р·Р° РІСЃС– РїРѕР·РёС†С–С—.</returns>
    public decimal TotalOf(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        decimal total = 0m;
        foreach (LoanItem item in loan.Items)
        {
            total += _feePolicy.PriceOf(item);
        }

        return Money.Round(total);
    }
}
