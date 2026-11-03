namespace LibraryDesk.Core.Lending;

/// <summary>Іменовані правила нарахування знижок і плати за доставку.</summary>
public static class LoanPricingRules
{
    /// <summary>Кількість примірників, з якої діє гуртова знижка.</summary>
    public const int BulkQuantity = 10;

    /// <summary>Частка гуртової знижки.</summary>
    public const decimal BulkRate = 0.05m;

    /// <summary>Частка знижки постійного читача.</summary>
    public const decimal RegularClientRate = 0.07m;

    /// <summary>Сума, понад яку діє знижка на велику видачу.</summary>
    public const decimal LargeOrderFrom = 5000m;

    /// <summary>Частка знижки на велику видачу.</summary>
    public const decimal LargeOrderRate = 0.10m;

    /// <summary>Сума, понад яку діє знижка на середню видачу.</summary>
    public const decimal MediumOrderFrom = 2000m;

    /// <summary>Частка знижки на середню видачу.</summary>
    public const decimal MediumOrderRate = 0.05m;

    /// <summary>Частка знижки вихідного дня.</summary>
    public const decimal WeekendRate = 0.02m;

    /// <summary>Частка знижки за купоном відсоткового типу.</summary>
    public const decimal CouponSaleRate = 0.10m;

    /// <summary>Сума знижки за купоном із фіксованою знижкою.</summary>
    public const decimal CouponFixed = 200m;

    /// <summary>Сума, понад яку діє купон із фіксованою знижкою.</summary>
    public const decimal CouponFixedFrom = 1000m;

    /// <summary>Сума, від якої доставка безкоштовна.</summary>
    public const decimal FreeDeliveryFrom = 1000m;

    /// <summary>Код купона з відсотковою знижкою.</summary>
    public const string SaleCoupon = "SALE10";

    /// <summary>Код купона з фіксованою знижкою.</summary>
    public const string FixedCoupon = "MINUS200";
}
