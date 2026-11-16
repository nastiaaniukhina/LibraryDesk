namespace LibraryDesk.Core.Pricing;

/// <summary>Пеня за прострочення повернення книжки.</summary>
public sealed class LateFeePolicy
{
    /// <summary>Пільгові доби, за які пеню не нараховують.</summary>
    public const int GraceDays = 3;

    /// <summary>Пеня за добу, грн.</summary>
    public const decimal DailyFee = 2m;

    /// <summary>Доплата за добу при тривалому простроченні, грн.</summary>
    public const decimal LongOverdueExtra = 1m;

    /// <summary>Кількість діб, з якої прострочення вважають тривалим.</summary>
    public const int LongOverdueFrom = 14;

    /// <summary>Множник пені для студента.</summary>
    public const decimal StudentMultiplier = 0.5m;

    /// <summary>Рахує пеню за прострочення.</summary>
    /// <param name="daysOverdue">Кількість діб прострочення, не від'ємна.</param>
    /// <param name="isStudent">Чи є читач студентом.</param>
    /// <param name="bookPrice">Вартість книжки, грн; верхня межа пені.</param>
    /// <returns>Сума пені, грн.</returns>
    public static decimal FeeFor(int daysOverdue, bool isStudent, decimal bookPrice = decimal.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(daysOverdue);
        if (daysOverdue <= GraceDays)
        {
            return 0m;
        }

        decimal rate = DailyFee;
        if (daysOverdue >= LongOverdueFrom)
        {
            rate += LongOverdueExtra;
        }

        decimal fee = (daysOverdue - GraceDays) * rate;
        if (isStudent)
        {
            fee *= StudentMultiplier;
        }

        return decimal.Round(fee, 2);
    }
}
