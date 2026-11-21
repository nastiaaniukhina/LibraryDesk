using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Core.Abstractions;

/// <summary>РљР°С‚Р°Р»РѕРі РїСЂРѕС„С–Р»С–РІ СЂРѕР·СЂР°С…СѓРЅРєСѓ РґР»СЏ РєР°С‚РµРіРѕСЂС–Р№ С‡РёС‚Р°С‡С–РІ.</summary>
public interface IPricingCatalog
{
    /// <summary>РџРѕРІРµСЂС‚Р°С” РїСЂРѕС„С–Р»СЊ РґР»СЏ РєР°С‚РµРіРѕСЂС–С— С‡РёС‚Р°С‡Р°.</summary>
    /// <param name="kind">РљР°С‚РµРіРѕСЂС–СЏ С‡РёС‚Р°С‡Р°.</param>
    /// <returns>РџСЂРѕС„С–Р»СЊ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</returns>
    PricingProfile GetFor(ReaderKind kind);
}
