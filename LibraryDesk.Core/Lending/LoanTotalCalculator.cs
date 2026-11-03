namespace LibraryDesk.Core.Lending;

/// <summary>Розрахунок підсумку видачі з урахуванням знижок.</summary>
public static class LoanTotalCalculator
{
    /// <summary>Рахує суму рядків видачі з урахуванням гуртової знижки.</summary>
    /// <param name="lines">Рядки видачі, не порожній перелік.</param>
    /// <returns>Сума всіх рядків після гуртових знижок.</returns>
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

    /// <summary>Рахує знижку постійного читача.</summary>
    /// <param name="amount">Поточна сума, не від'ємна.</param>
    /// <param name="isRegularClient">Чи є читач постійним.</param>
    /// <returns>Сума знижки.</returns>
    public static decimal ClientDiscountOf(decimal amount, bool isRegularClient)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return isRegularClient
            ? amount * LoanPricingRules.RegularClientRate
            : 0m;
    }

    /// <summary>Рахує знижку за обсягом видачі.</summary>
    /// <param name="amount">Поточна сума, не від'ємна.</param>
    /// <returns>Сума знижки.</returns>
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

    /// <summary>Рахує знижку за кодом купона.</summary>
    /// <param name="amount">Поточна сума, не від'ємна.</param>
    /// <param name="couponCode">Код купона або null, якщо купона немає.</param>
    /// <returns>Сума знижки.</returns>
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

    /// <summary>Рахує знижку вихідного дня.</summary>
    /// <param name="amount">Поточна сума, не від'ємна.</param>
    /// <param name="createdAt">Дата створення видачі.</param>
    /// <returns>Сума знижки.</returns>
    public static decimal WeekendDiscountOf(decimal amount, DateOnly createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return createdAt.DayOfWeek == DayOfWeek.Sunday
            ? amount * LoanPricingRules.WeekendRate
            : 0m;
    }

    /// <summary>Рахує плату за доставку.</summary>
    /// <param name="amount">Поточна сума, не від'ємна.</param>
    /// <param name="deliveryPrice">Вартість доставки, не від'ємна.</param>
    /// <returns>Плата за доставку або нуль, якщо доставка безкоштовна.</returns>
    public static decimal DeliveryFeeOf(decimal amount, decimal deliveryPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentOutOfRangeException.ThrowIfNegative(deliveryPrice);
        return amount < LoanPricingRules.FreeDeliveryFrom
            ? deliveryPrice
            : 0m;
    }

    /// <summary>Визначає наступний стан формуляра видачі.</summary>
    /// <param name="current">Поточний стан.</param>
    /// <param name="total">Підсумок видачі, не від'ємний.</param>
    /// <returns>Наступний стан; якщо перехід неможливий, то поточний.</returns>
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

    /// <summary>Диспетчер: викликає кроки розрахунку в потрібному порядку.</summary>
    /// <param name="docId">Номер видачі.</param>
    /// <param name="clientName">Прізвище читача.</param>
    /// <param name="clientEmail">Пошта читача.</param>
    /// <param name="isRegular">Чи є читач постійним.</param>
    /// <param name="lines">Рядки видачі.</param>
    /// <param name="createdAt">Дата створення видачі.</param>
    /// <param name="couponCode">Код купона або null.</param>
    /// <param name="status">Поточний стан видачі.</param>
    /// <param name="printToConsole">Чи друкувати звіт у консоль.</param>
    /// <param name="deliveryPrice">Вартість доставки.</param>
    /// <param name="newStatus">Стан видачі після розрахунку.</param>
    /// <returns>Підсумок видачі.</returns>
    public static decimal Process(
        int docId,
        string clientName,
        string clientEmail,
        bool isRegular,
        IReadOnlyList<LoanLine> lines,
        DateOnly createdAt,
        string? couponCode,
        LoanFormStatus status,
        bool printToConsole,
        decimal deliveryPrice,
        out LoanFormStatus newStatus)
    {
        LoanGuards.EnsureHeaderValid(docId, clientName, clientEmail);
        decimal total = SubtotalOf(lines);
        total -= ClientDiscountOf(total, isRegular);
        total -= VolumeDiscountOf(total);
        total -= CouponDiscountOf(total, couponCode);
        total -= WeekendDiscountOf(total, createdAt);
        total += DeliveryFeeOf(total, deliveryPrice);
        total = status == LoanFormStatus.Cancelled
            ? 0m
            : Math.Round(Math.Max(total, 0m), 2);
        newStatus = NextStatusOf(status, total);

        if (printToConsole)
        {
            Console.WriteLine("Видача № " + docId);
            Console.WriteLine("Читач: " + clientName);
            foreach (LoanLine line in lines)
            {
                Console.WriteLine(line.Isbn + " x " + line.Quantity);
            }

            Console.WriteLine("Разом: " + total);
            Console.WriteLine("Стан: " + newStatus);
        }

        return total;
    }
}
