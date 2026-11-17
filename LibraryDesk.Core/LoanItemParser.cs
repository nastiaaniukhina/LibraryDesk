using System.Globalization;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Lending;

/// <summary>Розбирає текстові дані позиції видачі та перевіряє їх.</summary>
public static class LoanItemParser
{
    private const int MinDays = 1;
    private const int MaxDays = 365;
    private const int IsbnLength = 13;

    /// <summary>Створює позицію видачі з рядка формату ISBN;дні;тариф.</summary>
    /// <param name="input">Вхідний рядок.</param>
    /// <returns>Перевірена позиція видачі.</returns>
    public static LoanItem Parse(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        string[] parts = input.Split(';');

        if (parts.Length != 3)
        {
            throw new FormatException("Неправильний формат введення.");
        }

        string isbn = parts[0].Trim();
        string daysText = parts[1].Trim();
        string rateText = parts[2].Trim();

        if (isbn.Length != IsbnLength ||
            !isbn.All(char.IsDigit))
        {
            throw new FormatException("ISBN повинен містити 13 цифр.");
        }

        if (!int.TryParse(
            daysText,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int days) ||
            days < MinDays ||
            days > MaxDays)
        {
            throw new FormatException("Кількість днів має бути від 1 до 365.");
        }

        if (!decimal.TryParse(
            rateText,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out decimal dailyRate) ||
            dailyRate < 0)
        {
            throw new FormatException("Некоректна денна ставка.");
        }

        return new LoanItem(isbn, days, dailyRate);
    }
}
