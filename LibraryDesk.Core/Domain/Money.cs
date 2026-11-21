namespace LibraryDesk.Core.Domain;

/// <summary>
/// Р—Р°Р±РµР·РїРµС‡СѓС” РѕРєСЂСѓРіР»РµРЅРЅСЏ РіСЂРѕС€РѕРІРёС… Р·РЅР°С‡РµРЅСЊ.
/// </summary>
public static class Money
{
    /// <summary>
    /// РљС–Р»СЊРєС–СЃС‚СЊ Р·РЅР°РєС–РІ РїС–СЃР»СЏ РєРѕРјРё РґР»СЏ РіСЂРѕС€РѕРІРёС… Р·РЅР°С‡РµРЅСЊ.
    /// </summary>
    public const int Digits = 2;

    /// <summary>
    /// РћРєСЂСѓРіР»СЋС” РіСЂРѕС€РѕРІРµ Р·РЅР°С‡РµРЅРЅСЏ Р·Р° РїСЂР°РІРёР»РѕРј AwayFromZero.
    /// </summary>
    /// <param name="amount">РЎСѓРјР° РґР»СЏ РѕРєСЂСѓРіР»РµРЅРЅСЏ.</param>
    /// <returns>РћРєСЂСѓРіР»РµРЅР° СЃСѓРјР°.</returns>
    public static decimal Round(decimal amount)
    {
        return Math.Round(amount, Digits, MidpointRounding.AwayFromZero);
    }
}
