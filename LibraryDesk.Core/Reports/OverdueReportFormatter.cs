using System.Globalization;

namespace LibraryDesk.Core.Reports;

/// <summary>Р¤РѕСЂРјР°С‚СѓС” Р·РІС–С‚ Р±РѕСЂР¶РЅРёРєС–РІ РґР»СЏ С‚РµРєСЃС‚РѕРІРѕРіРѕ РІРёРІРµРґРµРЅРЅСЏ.</summary>
public static class OverdueReportFormatter
{
    /// <summary>РџРµСЂРµС‚РІРѕСЂСЋС” СЂСЏРґРєРё Р·РІС–С‚Сѓ РЅР° С‚РµРєСЃС‚.</summary>
    public static string Format(IReadOnlyList<DebtorLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return string.Join(Environment.NewLine, lines.Select(line =>
            string.Create(CultureInfo.InvariantCulture,
                $"Р’РёРґР°С‡Р° #{line.LoanId}; С‡РёС‚Р°С‡ #{line.ReaderId}; РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ: {line.OverdueDays}; РїРµРЅСЏ: {line.Penalty:F2}")));
    }
}
