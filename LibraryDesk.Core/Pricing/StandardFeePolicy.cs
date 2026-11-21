using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>РЎС‚Р°РЅРґР°СЂС‚РЅР° РїР»Р°С‚Р° Р±РµР· Р·РЅРёР¶РѕРє.</summary>
public sealed class StandardFeePolicy : IFeePolicy
{
    /// <inheritdoc/>
    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Amount;
    }
}
