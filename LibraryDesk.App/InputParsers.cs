using System.Globalization;

namespace LibraryDesk.App;

/// <summary>Перевіряє введення на межі консольного застосунку.</summary>
public static class InputParsers
{
    /// <summary>Розбирає додатний номер видачі.</summary>
    public static bool TryLoanId(string? input, out int id) =>
        int.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out id) && id > 0;

    /// <summary>Розбирає дату у форматі yyyy-MM-dd.</summary>
    public static bool TryDate(string? input, out DateOnly date) =>
        DateOnly.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
}