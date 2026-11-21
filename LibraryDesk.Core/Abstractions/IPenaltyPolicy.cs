using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>РџРѕР»С–С‚РёРєР° СЂРѕР·СЂР°С…СѓРЅРєСѓ РїРµРЅС– Р·Р° РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ.</summary>
public interface IPenaltyPolicy
{
    /// <summary>Р РѕР·СЂР°С…РѕРІСѓС” РїРµРЅСЋ Р·Р° РѕРґРЅСѓ РїРѕР·РёС†С–СЋ РІРёРґР°С‡С–.</summary>
    /// <param name="item">РџРѕР·РёС†С–СЏ РІРёРґР°С‡С–.</param>
    /// <param name="overdueDays">РљС–Р»СЊРєС–СЃС‚СЊ РґРЅС–РІ РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ.</param>
    /// <param name="isStudent">Р§Рё С” С‡РёС‚Р°С‡ СЃС‚СѓРґРµРЅС‚РѕРј.</param>
    /// <returns>РЎСѓРјР° РїРµРЅС–, РѕР±РјРµР¶РµРЅР° РІР°СЂС‚С–СЃС‚СЋ РїРѕР·РёС†С–С—.</returns>
    decimal PenaltyOf(LoanItem item, int overdueDays, bool isStudent);
}
