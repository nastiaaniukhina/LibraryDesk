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

        decimal raw = (daysOverdue - GraceDays) * RateFor(daysOverdue) * MultiplierFor(isStudent);

        return Math.Min(decimal.Round(raw, 2), bookPrice);
    }

    private static decimal RateFor(int daysOverdue) =>
        daysOverdue >= LongOverdueFrom ? DailyFee + LongOverdueExtra : DailyFee;

    private static decimal MultiplierFor(bool isStudent) =>
        isStudent ? StudentMultiplier : 1m;
}
