namespace LibraryDesk.Core.Domain;

/// <summary>РџС–РґСЃСѓРјРѕРє РїСЂРёР№РЅСЏС‚С‚СЏ РїРѕРІРµСЂРЅРµРЅРЅСЏ.</summary>
/// <param name="Total">Р—Р°РіР°Р»СЊРЅР° РІР°СЂС‚С–СЃС‚СЊ.</param>
/// <param name="Penalty">РќР°СЂР°С…РѕРІР°РЅР° РїРµРЅСЏ.</param>
/// <param name="OverdueDays">РљС–Р»СЊРєС–СЃС‚СЊ РґРЅС–РІ РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ.</param>
public sealed record ReturnReceipt(decimal Total, decimal Penalty, int OverdueDays);
