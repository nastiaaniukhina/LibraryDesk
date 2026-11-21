using System.Globalization;
using System.Text;
using LibraryDesk.Labs.Lending;

namespace LibraryDesk.Labs;

/// <summary>Р¤РѕСЂРјСѓРІР°РЅРЅСЏ С‚РµРєСЃС‚Сѓ Р·РІС–С‚Сѓ РїСЂРѕ РІРёРґР°С‡Сѓ.</summary>
public static class LoanReport
{
    /// <summary>Р‘СѓРґСѓС” С‚РµРєСЃС‚ Р·РІС–С‚Сѓ РїСЂРѕ РІРёРґР°С‡Сѓ, РЅС–С‡РѕРіРѕ РЅРµ РІРёРІРѕРґСЏС‡Рё.</summary>
    /// <param name="request">Р’С…С–РґРЅС– РґР°РЅС– РІРёРґР°С‡С–.</param>
    /// <param name="result">Р РµР·СѓР»СЊС‚Р°С‚ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</param>
    /// <returns>РўРµРєСЃС‚ Р·РІС–С‚Сѓ.</returns>
    public static string TextOf(LoanTotalRequest request, LoanTotalResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(request.Reader);
        ArgumentNullException.ThrowIfNull(request.Lines);
        CultureInfo culture = CultureInfo.InvariantCulture;
        StringBuilder text = new();
        text.AppendLine(culture, $"Р’РёРґР°С‡Р° в„– {request.LoanId}");
        text.AppendLine(culture, $"Р§РёС‚Р°С‡: {request.Reader.Name}");
        foreach (LoanLine line in request.Lines)
        {
            text.AppendLine(culture, $"{line.Isbn} x {line.Quantity}");
        }

        text.AppendLine(culture, $"Р Р°Р·РѕРј: {result.Total:F2}");
        text.AppendLine(culture, $"РЎС‚Р°РЅ: {result.Status}");
        return text.ToString();
    }
}
