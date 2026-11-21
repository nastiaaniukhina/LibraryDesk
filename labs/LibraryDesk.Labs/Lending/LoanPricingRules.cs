namespace LibraryDesk.Labs.Lending;

/// <summary>Р†РјРµРЅРѕРІР°РЅС– РїСЂР°РІРёР»Р° РЅР°СЂР°С…СѓРІР°РЅРЅСЏ Р·РЅРёР¶РѕРє С– РїР»Р°С‚Рё Р·Р° РґРѕСЃС‚Р°РІРєСѓ.</summary>
public static class LoanPricingRules
{
    /// <summary>РљС–Р»СЊРєС–СЃС‚СЊ РїСЂРёРјС–СЂРЅРёРєС–РІ, Р· СЏРєРѕС— РґС–С” РіСѓСЂС‚РѕРІР° Р·РЅРёР¶РєР°.</summary>
    public const int BulkQuantity = 10;

    /// <summary>Р§Р°СЃС‚РєР° РіСѓСЂС‚РѕРІРѕС— Р·РЅРёР¶РєРё.</summary>
    public const decimal BulkRate = 0.05m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РїРѕСЃС‚С–Р№РЅРѕРіРѕ С‡РёС‚Р°С‡Р°.</summary>
    public const decimal RegularClientRate = 0.07m;

    /// <summary>РЎСѓРјР°, РїРѕРЅР°Рґ СЏРєСѓ РґС–С” Р·РЅРёР¶РєР° РЅР° РІРµР»РёРєСѓ РІРёРґР°С‡Сѓ.</summary>
    public const decimal LargeOrderFrom = 5000m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РЅР° РІРµР»РёРєСѓ РІРёРґР°С‡Сѓ.</summary>
    public const decimal LargeOrderRate = 0.10m;

    /// <summary>РЎСѓРјР°, РїРѕРЅР°Рґ СЏРєСѓ РґС–С” Р·РЅРёР¶РєР° РЅР° СЃРµСЂРµРґРЅСЋ РІРёРґР°С‡Сѓ.</summary>
    public const decimal MediumOrderFrom = 2000m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РЅР° СЃРµСЂРµРґРЅСЋ РІРёРґР°С‡Сѓ.</summary>
    public const decimal MediumOrderRate = 0.05m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РІРёС…С–РґРЅРѕРіРѕ РґРЅСЏ.</summary>
    public const decimal WeekendRate = 0.02m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё Р·Р° РєСѓРїРѕРЅРѕРј РІС–РґСЃРѕС‚РєРѕРІРѕРіРѕ С‚РёРїСѓ.</summary>
    public const decimal CouponSaleRate = 0.10m;

    /// <summary>РЎСѓРјР° Р·РЅРёР¶РєРё Р·Р° РєСѓРїРѕРЅРѕРј С–Р· С„С–РєСЃРѕРІР°РЅРѕСЋ Р·РЅРёР¶РєРѕСЋ.</summary>
    public const decimal CouponFixed = 200m;

    /// <summary>РЎСѓРјР°, РїРѕРЅР°Рґ СЏРєСѓ РґС–С” РєСѓРїРѕРЅ С–Р· С„С–РєСЃРѕРІР°РЅРѕСЋ Р·РЅРёР¶РєРѕСЋ.</summary>
    public const decimal CouponFixedFrom = 1000m;

    /// <summary>РЎСѓРјР°, РІС–Рґ СЏРєРѕС— РґРѕСЃС‚Р°РІРєР° Р±РµР·РєРѕС€С‚РѕРІРЅР°.</summary>
    public const decimal FreeDeliveryFrom = 1000m;

    /// <summary>РљРѕРґ РєСѓРїРѕРЅР° Р· РІС–РґСЃРѕС‚РєРѕРІРѕСЋ Р·РЅРёР¶РєРѕСЋ.</summary>
    public const string SaleCoupon = "SALE10";

    /// <summary>РљРѕРґ РєСѓРїРѕРЅР° Р· С„С–РєСЃРѕРІР°РЅРѕСЋ Р·РЅРёР¶РєРѕСЋ.</summary>
    public const string FixedCoupon = "MINUS200";
}
