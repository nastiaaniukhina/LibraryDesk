namespace LibraryDesk.Labs.Lending;

/// <summary>РЎС‚Р°РЅ С„РѕСЂРјСѓР»СЏСЂР° РІРёРґР°С‡С–.</summary>
public enum LoanFormStatus
{
    /// <summary>РљРЅРёР¶РєСѓ Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅРѕ.</summary>
    Reserved,

    /// <summary>РљРЅРёР¶РєСѓ РІРёРґР°РЅРѕ С‡РёС‚Р°С‡Сѓ.</summary>
    Issued,

    /// <summary>РљРЅРёР¶РєСѓ РїРѕРІРµСЂРЅРµРЅРѕ.</summary>
    Returned,

    /// <summary>Р’РёРґР°С‡Сѓ СЃРєР°СЃРѕРІР°РЅРѕ.</summary>
    Cancelled,
}
