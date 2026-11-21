using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>Каталог профілів розрахунку.</summary>
public sealed class PricingCatalog : IPricingCatalog
{
    private readonly IReadOnlyDictionary<ReaderKind, PricingProfile> _profiles;

    /// <summary>Створює каталог профілів.</summary>
    /// <param name="profiles">Профілі за категоріями читачів.</param>
    public PricingCatalog(IReadOnlyDictionary<ReaderKind, PricingProfile> profiles)
    {
        ArgumentNullException.ThrowIfNull(profiles);
        _profiles = profiles;
    }

    /// <inheritdoc/>
    public PricingProfile For(ReaderKind kind)
    {
        return _profiles.TryGetValue(kind, out PricingProfile? profile)
            ? profile
            : throw new ArgumentOutOfRangeException(nameof(kind));
    }

    /// <summary>Створює стандартний набір політик.</summary>
    /// <returns>Каталог для Regular, Student і Staff.</returns>
    public static PricingCatalog Default()
    {
        TieredPenaltyPolicy penalty = new();
        return new PricingCatalog(new Dictionary<ReaderKind, PricingProfile>
        {
            [ReaderKind.Regular] = new(new StandardFeePolicy(), penalty),
            [ReaderKind.Student] = new(new StudentFeePolicy(), penalty),
            [ReaderKind.Staff] = new(new DiscountFeePolicy(0.30m), penalty),
        });
    }
}