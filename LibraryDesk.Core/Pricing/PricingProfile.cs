using LibraryDesk.Core.Abstractions;

namespace LibraryDesk.Core.Pricing;

/// <summary>Набір політик вартості та пені для категорії читача.</summary>
/// <param name="Fee">Політика розрахунку вартості.</param>
/// <param name="Penalty">Політика розрахунку пені.</param>
public sealed record PricingProfile(IFeePolicy Fee, IPenaltyPolicy Penalty);