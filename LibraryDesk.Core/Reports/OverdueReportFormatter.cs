using System.Globalization;

namespace LibraryDesk.Core.Reports;

/// <summary>Форматує звіт боржників для текстового виведення.</summary>
public static class OverdueReportFormatter
{
    /// <summary>Перетворює рядки звіту на текст.</summary>
    public static string Format(IReadOnlyList<DebtorLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return string.Join(Environment.NewLine, lines.Select(line =>
            string.Create(CultureInfo.InvariantCulture,
                $"Видача #{line.LoanId}; читач #{line.ReaderId}; прострочення: {line.OverdueDays}; пеня: {line.Penalty:F2}")));
    }
}