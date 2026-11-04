using System.Globalization;
using System.Text;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Reports;

/// <summary>Формує CSV-звіт про видачі; нічого не пише на диск.</summary>
public static class LoanCsvReport
{
    /// <summary>Будує текст CSV.</summary>
    /// <param name="loans">Видачі для звіту.</param>
    /// <param name="totalOf">Функція розрахунку суми видачі.</param>
    /// <returns>Текст CSV із заголовком.</returns>
    public static string BuildCsv(IEnumerable<Loan> loans, Func<Loan, decimal> totalOf)
    {
        ArgumentNullException.ThrowIfNull(loans);
        ArgumentNullException.ThrowIfNull(totalOf);

        CultureInfo culture = CultureInfo.InvariantCulture;
        StringBuilder text = new();
        text.AppendLine("id;total;status");
        foreach (Loan loan in loans)
        {
            text.AppendLine(culture, $"{loan.Id};{totalOf(loan):F2};{loan.Status}");
        }

        return text.ToString();
    }
}
