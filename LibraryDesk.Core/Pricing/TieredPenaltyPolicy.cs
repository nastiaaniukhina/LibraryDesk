using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>Р‘Р°РіР°С‚РѕСЂС–РІРЅРµРІР° РїРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РїРµРЅС–.</summary>
public sealed class TieredPenaltyPolicy : IPenaltyPolicy
{
    /// <inheritdoc/>
    public decimal PenaltyOf(LoanItem item, int overdueDays, bool isStudent)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfNegative(overdueDays);

        if (overdueDays == 0)
        {
            return 0m;
        }

        int firstTierDays = Math.Min(overdueDays, 7);
        int remainingDays = Math.Max(overdueDays - 7, 0);

        decimal firstTierMultiplier = isStudent ? 0.25m : 0.5m;
        decimal secondTierMultiplier = isStudent ? 0.5m : 1m;

        decimal firstTierPenalty = firstTierDays * item.DailyRate * firstTierMultiplier;
        decimal secondTierPenalty = remainingDays * item.DailyRate * secondTierMultiplier;
        decimal penalty = firstTierPenalty + secondTierPenalty;

        return Math.Min(penalty, item.Amount);
    }
}
