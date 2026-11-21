using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>РџРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РїР»Р°С‚Рё Р·Р° РїРѕР·РёС†С–СЋ РІРёРґР°С‡С–.</summary>
public interface IFeePolicy
{
    /// <summary>Р Р°С…СѓС” РїР»Р°С‚Сѓ Р·Р° РѕРґРЅСѓ РїРѕР·РёС†С–СЋ.</summary>
    /// <param name="item">РџРѕР·РёС†С–СЏ РІРёРґР°С‡С–.</param>
    /// <returns>РџР»Р°С‚Р° Р·Р° РїРѕР·РёС†С–СЋ, РіСЂРЅ.</returns>
    decimal PriceOf(LoanItem item);
}
