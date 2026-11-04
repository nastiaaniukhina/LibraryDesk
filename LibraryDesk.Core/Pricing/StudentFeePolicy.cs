using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>Плата для студентів: знижка 10 %.</summary>
public sealed class StudentFeePolicy : IFeePolicy
{
    private const decimal Multiplier = 0.90m;

    /// <inheritdoc/>
    public decimal PriceOf(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Math.Round(item.Amount * Multiplier, 2);
    }
}
