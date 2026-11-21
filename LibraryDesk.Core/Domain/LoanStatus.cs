namespace LibraryDesk.Core.Domain;

/// <summary>РЎС‚Р°РЅ РІРёРґР°С‡С– РєРЅРёР¶РѕРє Сѓ С—С— Р¶РёС‚С‚С”РІРѕРјСѓ С†РёРєР»С–.</summary>
public enum LoanStatus
{
    /// <summary>РљРЅРёР¶РєРё Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅРѕ.</summary>
    Reserved,

    /// <summary>РљРЅРёР¶РєРё РІРёРґР°РЅРѕ С‡РёС‚Р°С‡РµРІС–.</summary>
    Issued,

    /// <summary>РљРЅРёР¶РєРё РїРѕРІРµСЂРЅРµРЅРѕ.</summary>
    Returned,

    /// <summary>РЎС‚СЂРѕРє РїРѕРІРµСЂРЅРµРЅРЅСЏ РјРёРЅСѓРІ.</summary>
    Overdue,

    /// <summary>Р‘СЂРѕРЅСЊ СЃРєР°СЃРѕРІР°РЅРѕ РґРѕ РІРёРґР°С‡С–.</summary>
    Cancelled
}
