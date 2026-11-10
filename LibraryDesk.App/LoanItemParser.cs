using System.Globalization;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Errors;

namespace LibraryDesk.App;

/// <summary>Розбирач користувацького вводу: барикада між консоллю та ядром.</summary>
public static class LoanItemParser
{
    /// <summary>Розбирає рядки консолі в позицію видачі.</summary>
    /// <param name="isbn">ISBN книжки.</param>
    /// <param name="days">Кількість днів видачі.</param>
    /// <param name="dailyRate">Добова ставка; дробова частина через крапку.</param>
    /// <returns>Позиція або пояснення помилки.</returns>
    public static Result<LoanItem> Parse(string? isbn, string? days, string? dailyRate)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            return Result<LoanItem>.Fail("ISBN порожній");
        }

        if (!int.TryParse(days, NumberStyles.Integer, CultureInfo.InvariantCulture, out int d) || d <= 0)
        {
            return Result<LoanItem>.Fail("Кількість днів — ціле число більше 0");
        }

        if (!decimal.TryParse(dailyRate, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal r) || r < 0)
        {
            return Result<LoanItem>.Fail("Ставка — число не менше 0, дробова частина через крапку");
        }

        return Result<LoanItem>.Ok(new LoanItem(isbn.Trim(), d, r));
    }
}
