using LibraryDesk.Core.Abstractions;

namespace LibraryDesk.Core.Pricing;

/// <summary>РќР°Р±С–СЂ РїРѕР»С–С‚РёРє РІР°СЂС‚РѕСЃС‚С– С‚Р° РїРµРЅС– РґР»СЏ РєР°С‚РµРіРѕСЂС–С— С‡РёС‚Р°С‡Р°.</summary>
/// <param name="Fee">РџРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РІР°СЂС‚РѕСЃС‚С–.</param>
/// <param name="Penalty">РџРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РїРµРЅС–.</param>
public sealed record PricingProfile(IFeePolicy Fee, IPenaltyPolicy Penalty);
