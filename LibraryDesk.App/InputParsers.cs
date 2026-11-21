using System.Globalization;

namespace LibraryDesk.App;

/// <summary>РџРµСЂРµРІС–СЂСЏС” РІРІРµРґРµРЅРЅСЏ РЅР° РјРµР¶С– РєРѕРЅСЃРѕР»СЊРЅРѕРіРѕ Р·Р°СЃС‚РѕСЃСѓРЅРєСѓ.</summary>
public static class InputParsers
{
    /// <summary>Р РѕР·Р±РёСЂР°С” РґРѕРґР°С‚РЅРёР№ РЅРѕРјРµСЂ РІРёРґР°С‡С–.</summary>
    public static bool TryLoanId(string? input, out int id) =>
        int.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out id) && id > 0;

    /// <summary>Р РѕР·Р±РёСЂР°С” РґР°С‚Сѓ Сѓ С„РѕСЂРјР°С‚С– yyyy-MM-dd.</summary>
    public static bool TryDate(string? input, out DateOnly date) =>
        DateOnly.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
}
