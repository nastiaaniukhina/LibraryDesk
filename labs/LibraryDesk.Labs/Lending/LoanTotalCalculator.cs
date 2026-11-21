namespace LibraryDesk.Labs.Lending;

/// <summary>Р РѕР·СЂР°С…СѓРЅРѕРє РїС–РґСЃСѓРјРєСѓ РІРёРґР°С‡С– Р· СѓСЂР°С…СѓРІР°РЅРЅСЏРј Р·РЅРёР¶РѕРє.</summary>
public static class LoanTotalCalculator
{
    /// <summary>Р Р°С…СѓС” СЃСѓРјСѓ СЂСЏРґРєС–РІ РІРёРґР°С‡С– Р· СѓСЂР°С…СѓРІР°РЅРЅСЏРј РіСѓСЂС‚РѕРІРѕС— Р·РЅРёР¶РєРё.</summary>
    /// <param name="lines">Р СЏРґРєРё РІРёРґР°С‡С–, РЅРµ РїРѕСЂРѕР¶РЅС–Р№ РїРµСЂРµР»С–Рє.</param>
    /// <returns>РЎСѓРјР° РІСЃС–С… СЂСЏРґРєС–РІ РїС–СЃР»СЏ РіСѓСЂС‚РѕРІРёС… Р·РЅРёР¶РѕРє.</returns>
    public static decimal SubtotalOf(IReadOnlyList<LoanLine> lines)
    {
        LoanGuards.EnsureLinesValid(lines);
        decimal subtotal = 0m;
        foreach (LoanLine line in lines)
        {
            decimal sum = line.Quantity * line.UnitPrice;
            if (line.Quantity >= LoanPricingRules.BulkQuantity)
            {
                sum -= sum * LoanPricingRules.BulkRate;
            }

            subtotal += sum;
        }

        return subtotal;
    }

    /// <summary>Р Р°С…СѓС” Р·РЅРёР¶РєСѓ РїРѕСЃС‚С–Р№РЅРѕРіРѕ С‡РёС‚Р°С‡Р°.</summary>
    /// <param name="amount">РџРѕС‚РѕС‡РЅР° СЃСѓРјР°, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <param name="isRegularClient">Р§Рё С” С‡РёС‚Р°С‡ РїРѕСЃС‚С–Р№РЅРёРј.</param>
    /// <returns>РЎСѓРјР° Р·РЅРёР¶РєРё.</returns>
    public static decimal ClientDiscountOf(decimal amount, bool isRegularClient)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return isRegularClient
            ? amount * LoanPricingRules.RegularClientRate
            : 0m;
    }

    /// <summary>Р Р°С…СѓС” Р·РЅРёР¶РєСѓ Р·Р° РѕР±СЃСЏРіРѕРј РІРёРґР°С‡С–.</summary>
    /// <param name="amount">РџРѕС‚РѕС‡РЅР° СЃСѓРјР°, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <returns>РЎСѓРјР° Р·РЅРёР¶РєРё.</returns>
    public static decimal VolumeDiscountOf(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        if (amount > LoanPricingRules.LargeOrderFrom)
        {
            return amount * LoanPricingRules.LargeOrderRate;
        }

        if (amount > LoanPricingRules.MediumOrderFrom)
        {
            return amount * LoanPricingRules.MediumOrderRate;
        }

        return 0m;
    }

    /// <summary>Р Р°С…СѓС” Р·РЅРёР¶РєСѓ Р·Р° РєРѕРґРѕРј РєСѓРїРѕРЅР°.</summary>
    /// <param name="amount">РџРѕС‚РѕС‡РЅР° СЃСѓРјР°, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <param name="couponCode">РљРѕРґ РєСѓРїРѕРЅР° Р°Р±Рѕ null, СЏРєС‰Рѕ РєСѓРїРѕРЅР° РЅРµРјР°С”.</param>
    /// <returns>РЎСѓРјР° Р·РЅРёР¶РєРё.</returns>
    public static decimal CouponDiscountOf(decimal amount, string? couponCode)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return couponCode switch
        {
            LoanPricingRules.SaleCoupon
                => amount * LoanPricingRules.CouponSaleRate,
            LoanPricingRules.FixedCoupon
                when amount > LoanPricingRules.CouponFixedFrom
                => LoanPricingRules.CouponFixed,
            _ => 0m,
        };
    }

    /// <summary>Р Р°С…СѓС” Р·РЅРёР¶РєСѓ РІРёС…С–РґРЅРѕРіРѕ РґРЅСЏ.</summary>
    /// <param name="amount">РџРѕС‚РѕС‡РЅР° СЃСѓРјР°, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <param name="createdAt">Р”Р°С‚Р° СЃС‚РІРѕСЂРµРЅРЅСЏ РІРёРґР°С‡С–.</param>
    /// <returns>РЎСѓРјР° Р·РЅРёР¶РєРё.</returns>
    public static decimal WeekendDiscountOf(decimal amount, DateOnly createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return createdAt.DayOfWeek == DayOfWeek.Sunday
            ? amount * LoanPricingRules.WeekendRate
            : 0m;
    }

    /// <summary>Р Р°С…СѓС” РїР»Р°С‚Сѓ Р·Р° РґРѕСЃС‚Р°РІРєСѓ.</summary>
    /// <param name="amount">РџРѕС‚РѕС‡РЅР° СЃСѓРјР°, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <param name="deliveryPrice">Р’Р°СЂС‚С–СЃС‚СЊ РґРѕСЃС‚Р°РІРєРё, РЅРµ РІС–Рґ'С”РјРЅР°.</param>
    /// <returns>РџР»Р°С‚Р° Р·Р° РґРѕСЃС‚Р°РІРєСѓ Р°Р±Рѕ РЅСѓР»СЊ, СЏРєС‰Рѕ РґРѕСЃС‚Р°РІРєР° Р±РµР·РєРѕС€С‚РѕРІРЅР°.</returns>
    public static decimal DeliveryFeeOf(decimal amount, decimal deliveryPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentOutOfRangeException.ThrowIfNegative(deliveryPrice);
        return amount < LoanPricingRules.FreeDeliveryFrom
            ? deliveryPrice
            : 0m;
    }

    /// <summary>Р’РёР·РЅР°С‡Р°С” РЅР°СЃС‚СѓРїРЅРёР№ СЃС‚Р°РЅ С„РѕСЂРјСѓР»СЏСЂР° РІРёРґР°С‡С–.</summary>
    /// <param name="current">РџРѕС‚РѕС‡РЅРёР№ СЃС‚Р°РЅ.</param>
    /// <param name="total">РџС–РґСЃСѓРјРѕРє РІРёРґР°С‡С–, РЅРµ РІС–Рґ'С”РјРЅРёР№.</param>
    /// <returns>РќР°СЃС‚СѓРїРЅРёР№ СЃС‚Р°РЅ; СЏРєС‰Рѕ РїРµСЂРµС…С–Рґ РЅРµРјРѕР¶Р»РёРІРёР№, С‚Рѕ РїРѕС‚РѕС‡РЅРёР№.</returns>
    public static LoanFormStatus NextStatusOf(LoanFormStatus current, decimal total)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(total);
        return current switch
        {
            LoanFormStatus.Reserved when total > 0m => LoanFormStatus.Issued,
            LoanFormStatus.Issued => LoanFormStatus.Returned,
            _ => current,
        };
    }

    /// <summary>Р Р°С…СѓС” РїС–РґСЃСѓРјРѕРє РІРёРґР°С‡С– Р№ РЅР°СЃС‚СѓРїРЅРёР№ СЃС‚Р°РЅ С„РѕСЂРјСѓР»СЏСЂР°.</summary>
    /// <param name="request">Р—Р°РїРёС‚ РЅР° СЂРѕР·СЂР°С…СѓРЅРѕРє.</param>
    /// <returns>РџС–РґСЃСѓРјРѕРє С– СЃС‚Р°РЅ РІРёРґР°С‡С– РїС–СЃР»СЏ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</returns>
    public static LoanTotalResult Calculate(LoanTotalRequest request)
    {
        LoanGuards.EnsureRequestValid(request);

        decimal total = SubtotalOf(request.Lines);
        total -= ClientDiscountOf(total, request.Reader.IsRegular);
        total -= VolumeDiscountOf(total);
        total -= CouponDiscountOf(total, request.CouponCode);
        total -= WeekendDiscountOf(total, request.CreatedAt);
        total += DeliveryFeeOf(total, request.DeliveryPrice);
        total = request.Status == LoanFormStatus.Cancelled
            ? 0m
            : Math.Round(Math.Max(total, 0m), 2);
        return new LoanTotalResult(
            total, NextStatusOf(request.Status, total));
    }
}
