using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>Плата зі знижкою за заданою ставкою.</summary>
public sealed class DiscountFeePolicy : IFeePolicy
{
    private const decimal MaxRate = 0.5m;

    private readonly decimal _rate;

    /// <summary>Initializes a new instance of the <see cref="DiscountFeePolicy"/> class.</summary>
    /// <param name="rate">Ставка знижки від 0 до 0,5.</param>
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
        return Math.Round(item.Amount * (1m - _rate), 2);
    }
}
