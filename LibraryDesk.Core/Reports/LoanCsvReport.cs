using System.Globalization;
using System.Text;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Reports;

/// <summary>Р¤РѕСЂРјСѓС” CSV-Р·РІС–С‚ РїСЂРѕ РІРёРґР°С‡С–; РЅС–С‡РѕРіРѕ РЅРµ РїРёС€Рµ РЅР° РґРёСЃРє.</summary>
public static class LoanCsvReport
{
    /// <summary>Р‘СѓРґСѓС” С‚РµРєСЃС‚ CSV.</summary>
    /// <param name="loans">Р’РёРґР°С‡С– РґР»СЏ Р·РІС–С‚Сѓ.</param>
    /// <param name="totalOf">Р¤СѓРЅРєС†С–СЏ СЂРѕР·СЂР°С…СѓРЅРєСѓ СЃСѓРјРё РІРёРґР°С‡С–.</param>
    /// <returns>РўРµРєСЃС‚ CSV С–Р· Р·Р°РіРѕР»РѕРІРєРѕРј.</returns>
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
