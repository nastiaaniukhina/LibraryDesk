using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>Стандартна плата без знижок.</summary>
public sealed class StandardFeePolicy : IFeePolicy
{
    /// <inheritdoc/>
    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Amount;
    }
}
