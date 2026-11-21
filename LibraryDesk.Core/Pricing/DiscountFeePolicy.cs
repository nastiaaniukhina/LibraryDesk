using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>РџР»Р°С‚Р° Р·С– Р·РЅРёР¶РєРѕСЋ Р·Р° Р·Р°РґР°РЅРѕСЋ СЃС‚Р°РІРєРѕСЋ.</summary>
public sealed class DiscountFeePolicy : IFeePolicy
{
    private const decimal MaxRate = 0.5m;

    private readonly decimal _rate;

    /// <summary>Initializes a new instance of the <see cref="DiscountFeePolicy"/> class.</summary>
    /// <param name="rate">РЎС‚Р°РІРєР° Р·РЅРёР¶РєРё РІС–Рґ 0 РґРѕ 0,5.</param>
    public DiscountFeePolicy(decimal rate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rate, MaxRate);
        _rate = rate;
    }

    /// <inheritdoc/>
    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Amount * (1m - _rate);
    }
}
