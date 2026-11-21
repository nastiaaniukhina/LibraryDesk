using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Pricing;

/// <summary>РљР°С‚Р°Р»РѕРі РїСЂРѕС„С–Р»С–РІ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</summary>
public sealed class PricingCatalog : IPricingCatalog
{
    private readonly IReadOnlyDictionary<ReaderKind, PricingProfile> _profiles;

    /// <summary>РЎС‚РІРѕСЂСЋС” РєР°С‚Р°Р»РѕРі РїСЂРѕС„С–Р»С–РІ.</summary>
    /// <param name="profiles">РџСЂРѕС„С–Р»С– Р·Р° РєР°С‚РµРіРѕСЂС–СЏРјРё С‡РёС‚Р°С‡С–РІ.</param>
    public PricingCatalog(IReadOnlyDictionary<ReaderKind, PricingProfile> profiles)
    {
        ArgumentNullException.ThrowIfNull(profiles);
        _profiles = profiles;
    }

    /// <inheritdoc/>
    public PricingProfile GetFor(ReaderKind kind)
    {
        return _profiles.TryGetValue(kind, out PricingProfile? profile)
            ? profile
            : throw new ArgumentOutOfRangeException(nameof(kind));
    }

    /// <summary>РЎС‚РІРѕСЂСЋС” СЃС‚Р°РЅРґР°СЂС‚РЅРёР№ РЅР°Р±С–СЂ РїРѕР»С–С‚РёРє.</summary>
    /// <returns>РљР°С‚Р°Р»РѕРі РґР»СЏ Regular, Student С– Staff.</returns>
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
