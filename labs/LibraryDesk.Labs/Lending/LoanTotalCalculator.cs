namespace LibraryDesk.Labs.Lending;

/// <summary>Р В Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С•Р С” Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С”РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р В· РЎС“РЎР‚Р В°РЎвЂ¦РЎС“Р Р†Р В°Р Р…Р Р…РЎРЏР С Р В·Р Р…Р С‘Р В¶Р С•Р С”.</summary>
public static class LoanTotalCalculator
{
    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ РЎРѓРЎС“Р СРЎС“ РЎР‚РЎРЏР Т‘Р С”РЎвЂ“Р Р† Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р В· РЎС“РЎР‚Р В°РЎвЂ¦РЎС“Р Р†Р В°Р Р…Р Р…РЎРЏР С Р С–РЎС“РЎР‚РЎвЂљР С•Р Р†Р С•РЎвЂ” Р В·Р Р…Р С‘Р В¶Р С”Р С‘.</summary>
    /// <param name="lines">Р В РЎРЏР Т‘Р С”Р С‘ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, Р Р…Р Вµ Р С—Р С•РЎР‚Р С•Р В¶Р Р…РЎвЂ“Р в„– Р С—Р ВµРЎР‚Р ВµР В»РЎвЂ“Р С”.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р Р†РЎРѓРЎвЂ“РЎвЂ¦ РЎР‚РЎРЏР Т‘Р С”РЎвЂ“Р Р† Р С—РЎвЂ“РЎРѓР В»РЎРЏ Р С–РЎС“РЎР‚РЎвЂљР С•Р Р†Р С‘РЎвЂ¦ Р В·Р Р…Р С‘Р В¶Р С•Р С”.</returns>
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

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р В·Р Р…Р С‘Р В¶Р С”РЎС“ Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С•Р С–Р С• РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</summary>
    /// <param name="amount">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р В° РЎРѓРЎС“Р СР В°, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <param name="isRegularClient">Р В§Р С‘ РЎвЂќ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎ Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С‘Р С.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘.</returns>
    public static decimal ClientDiscountOf(decimal amount, bool isRegularClient)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return isRegularClient
            ? amount * LoanPricingRules.RegularClientRate
            : 0m;
    }

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р В·Р Р…Р С‘Р В¶Р С”РЎС“ Р В·Р В° Р С•Р В±РЎРѓРЎРЏР С–Р С•Р С Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    /// <param name="amount">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р В° РЎРѓРЎС“Р СР В°, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘.</returns>
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

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р В·Р Р…Р С‘Р В¶Р С”РЎС“ Р В·Р В° Р С”Р С•Р Т‘Р С•Р С Р С”РЎС“Р С—Р С•Р Р…Р В°.</summary>
    /// <param name="amount">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р В° РЎРѓРЎС“Р СР В°, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <param name="couponCode">Р С™Р С•Р Т‘ Р С”РЎС“Р С—Р С•Р Р…Р В° Р В°Р В±Р С• null, РЎРЏР С”РЎвЂ°Р С• Р С”РЎС“Р С—Р С•Р Р…Р В° Р Р…Р ВµР СР В°РЎвЂќ.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘.</returns>
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

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р В·Р Р…Р С‘Р В¶Р С”РЎС“ Р Р†Р С‘РЎвЂ¦РЎвЂ“Р Т‘Р Р…Р С•Р С–Р С• Р Т‘Р Р…РЎРЏ.</summary>
    /// <param name="amount">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р В° РЎРѓРЎС“Р СР В°, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <param name="createdAt">Р вЂќР В°РЎвЂљР В° РЎРѓРЎвЂљР Р†Р С•РЎР‚Р ВµР Р…Р Р…РЎРЏ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘.</returns>
    public static decimal WeekendDiscountOf(decimal amount, DateOnly createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return createdAt.DayOfWeek == DayOfWeek.Sunday
            ? amount * LoanPricingRules.WeekendRate
            : 0m;
    }

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р С—Р В»Р В°РЎвЂљРЎС“ Р В·Р В° Р Т‘Р С•РЎРѓРЎвЂљР В°Р Р†Р С”РЎС“.</summary>
    /// <param name="amount">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р В° РЎРѓРЎС“Р СР В°, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <param name="deliveryPrice">Р вЂ™Р В°РЎР‚РЎвЂљРЎвЂ“РЎРѓРЎвЂљРЎРЉ Р Т‘Р С•РЎРѓРЎвЂљР В°Р Р†Р С”Р С‘, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р В°.</param>
    /// <returns>Р СџР В»Р В°РЎвЂљР В° Р В·Р В° Р Т‘Р С•РЎРѓРЎвЂљР В°Р Р†Р С”РЎС“ Р В°Р В±Р С• Р Р…РЎС“Р В»РЎРЉ, РЎРЏР С”РЎвЂ°Р С• Р Т‘Р С•РЎРѓРЎвЂљР В°Р Р†Р С”Р В° Р В±Р ВµР В·Р С”Р С•РЎв‚¬РЎвЂљР С•Р Р†Р Р…Р В°.</returns>
    public static decimal DeliveryFeeOf(decimal amount, decimal deliveryPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentOutOfRangeException.ThrowIfNegative(deliveryPrice);
        return amount < LoanPricingRules.FreeDeliveryFrom
            ? deliveryPrice
            : 0m;
    }

    /// <summary>Р вЂ™Р С‘Р В·Р Р…Р В°РЎвЂЎР В°РЎвЂќ Р Р…Р В°РЎРѓРЎвЂљРЎС“Р С—Р Р…Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р… РЎвЂћР С•РЎР‚Р СРЎС“Р В»РЎРЏРЎР‚Р В° Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    /// <param name="current">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р….</param>
    /// <param name="total">Р СџРЎвЂ“Р Т‘РЎРѓРЎС“Р СР С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, Р Р…Р Вµ Р Р†РЎвЂ“Р Т‘'РЎвЂќР СР Р…Р С‘Р в„–.</param>
    /// <returns>Р СњР В°РЎРѓРЎвЂљРЎС“Р С—Р Р…Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р…; РЎРЏР С”РЎвЂ°Р С• Р С—Р ВµРЎР‚Р ВµРЎвЂ¦РЎвЂ“Р Т‘ Р Р…Р ВµР СР С•Р В¶Р В»Р С‘Р Р†Р С‘Р в„–, РЎвЂљР С• Р С—Р С•РЎвЂљР С•РЎвЂЎР Р…Р С‘Р в„–.</returns>
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

    /// <summary>Р В Р В°РЎвЂ¦РЎС“РЎвЂќ Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р в„– Р Р…Р В°РЎРѓРЎвЂљРЎС“Р С—Р Р…Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р… РЎвЂћР С•РЎР‚Р СРЎС“Р В»РЎРЏРЎР‚Р В°.</summary>
    /// <param name="request">Р вЂ”Р В°Р С—Р С‘РЎвЂљ Р Р…Р В° РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С•Р С”.</param>
    /// <returns>Р СџРЎвЂ“Р Т‘РЎРѓРЎС“Р СР С•Р С” РЎвЂ“ РЎРѓРЎвЂљР В°Р Р… Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р С—РЎвЂ“РЎРѓР В»РЎРЏ РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С”РЎС“.</returns>
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
