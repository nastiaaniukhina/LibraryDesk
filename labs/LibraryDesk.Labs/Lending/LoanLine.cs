namespace LibraryDesk.Labs.Lending;

/// <summary>Р СЏРґРѕРє С„РѕСЂРјСѓР»СЏСЂР° РІРёРґР°С‡С–: РєРЅРёР¶РєР°, РєС–Р»СЊРєС–СЃС‚СЊ РїСЂРёРјС–СЂРЅРёРєС–РІ С– С†С–РЅР°.</summary>
public sealed class LoanLine
{
    /// <summary>Gets iSBN-РєРѕРґ РєРЅРёР¶РєРё.</summary>
    public string Isbn { get; init; } = string.Empty;

    /// <summary>Gets РєС–Р»СЊРєС–СЃС‚СЊ РїСЂРёРјС–СЂРЅРёРєС–РІ.</summary>
    public int Quantity { get; init; }

    /// <summary>Gets С†С–РЅСѓ РѕРґРЅРѕРіРѕ РїСЂРёРјС–СЂРЅРёРєР°, РіСЂРЅ.</summary>
    public decimal UnitPrice { get; init; }
}
