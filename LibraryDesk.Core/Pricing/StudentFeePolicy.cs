using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>РџР»Р°С‚Р° РґР»СЏ СЃС‚СѓРґРµРЅС‚С–РІ: Р·РЅРёР¶РєР° 10 %.</summary>
public sealed class StudentFeePolicy : IFeePolicy
{
    private const decimal Multiplier = 0.90m;

    /// <inheritdoc/>
    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Amount * Multiplier;
    }
}
