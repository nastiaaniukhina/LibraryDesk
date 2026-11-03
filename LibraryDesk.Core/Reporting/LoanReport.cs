using System.Globalization;
using System.Text;
using LibraryDesk.Core.Lending;

namespace LibraryDesk.Core.Reporting;

/// <summary>Формування тексту звіту про видачу.</summary>
public static class LoanReport
{
    /// <summary>Будує текст звіту про видачу, нічого не виводячи.</summary>
    /// <param name="request">Вхідні дані видачі.</param>
    /// <param name="result">Результат розрахунку.</param>
    /// <returns>Текст звіту.</returns>
    public static string TextOf(LoanTotalRequest request, LoanTotalResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(request.Reader);
        ArgumentNullException.ThrowIfNull(request.Lines);
        CultureInfo culture = CultureInfo.InvariantCulture;
        StringBuilder text = new();
        text.AppendLine(culture, $"Видача № {request.LoanId}");
        text.AppendLine(culture, $"Читач: {request.Reader.Name}");
        foreach (LoanLine line in request.Lines)
        {
            text.AppendLine(culture, $"{line.Isbn} x {line.Quantity}");
        }

        text.AppendLine(culture, $"Разом: {result.Total:F2}");
        text.AppendLine(culture, $"Стан: {result.Status}");
        return text.ToString();
    }
}
