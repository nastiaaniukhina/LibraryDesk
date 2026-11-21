namespace LibraryDesk.Labs.Lending;

/// <summary>РџРµСЂРµРІС–СЂРєРё РїРµСЂРµРґСѓРјРѕРІ РґР»СЏ РґР°РЅРёС… РІРёРґР°С‡С–.</summary>
public static class LoanGuards
{
    /// <summary>РџРµСЂРµРІС–СЂСЏС” Р·Р°РіРѕР»РѕРІРѕРє РІРёРґР°С‡С–: РЅРѕРјРµСЂ, РїСЂС–Р·РІРёС‰Рµ С‡РёС‚Р°С‡Р°, РїРѕС€С‚Сѓ.</summary>
    /// <param name="loanId">РќРѕРјРµСЂ РІРёРґР°С‡С–, РјР°С” Р±СѓС‚Рё РґРѕРґР°С‚РЅРёРј.</param>
    /// <param name="readerName">РџСЂС–Р·РІРёС‰Рµ С‡РёС‚Р°С‡Р°, РЅРµ РїРѕСЂРѕР¶РЅС”.</param>
    /// <param name="readerEmail">РџРѕС€С‚Р° С‡РёС‚Р°С‡Р°, РјС–СЃС‚РёС‚СЊ СЃРёРјРІРѕР» @.</param>
    public static void EnsureHeaderValid(
        int loanId, string readerName, string readerEmail)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(loanId);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerEmail);
        if (!readerEmail.Contains('@'))
        {
            throw new ArgumentException(
                "РџРѕС€С‚Р° РјР°С” РјС–СЃС‚РёС‚Рё СЃРёРјРІРѕР» @.",
                nameof(readerEmail));
        }
    }

    /// <summary>РџРµСЂРµРІС–СЂСЏС” РѕРґРёРЅ СЂСЏРґРѕРє РІРёРґР°С‡С–.</summary>
    /// <param name="line">Р СЏРґРѕРє РІРёРґР°С‡С–: ISBN, РєС–Р»СЊРєС–СЃС‚СЊ, С†С–РЅР°.</param>
    public static void EnsureLineValid(LoanLine line)
    {
        ArgumentNullException.ThrowIfNull(line);
        ArgumentException.ThrowIfNullOrWhiteSpace(line.Isbn);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            line.Quantity, nameof(line.Quantity));
        ArgumentOutOfRangeException.ThrowIfNegative(
            line.UnitPrice, nameof(line.UnitPrice));
    }

    /// <summary>РџРµСЂРµРІС–СЂСЏС” РїРµСЂРµР»С–Рє СЂСЏРґРєС–РІ РІРёРґР°С‡С–.</summary>
    /// <param name="lines">Р СЏРґРєРё РІРёРґР°С‡С–, РїРµСЂРµР»С–Рє РЅРµ РїРѕСЂРѕР¶РЅС–Р№.</param>
    public static void EnsureLinesValid(IReadOnlyList<LoanLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        if (lines.Count == 0)
        {
            throw new ArgumentException(
                "Р’РёРґР°С‡Р° РЅРµ РјС–СЃС‚РёС‚СЊ Р¶РѕРґРЅРѕС— РєРЅРёР¶РєРё.",
                nameof(lines));
        }

        foreach (LoanLine line in lines)
        {
            EnsureLineValid(line);
        }
    }

    /// <summary>РџРµСЂРµРІС–СЂСЏС” РІРµСЃСЊ Р·Р°РїРёС‚ РЅР° СЂРѕР·СЂР°С…СѓРЅРѕРє РїС–РґСЃСѓРјРєСѓ.</summary>
    /// <param name="request">Р—Р°РїРёС‚ РЅР° СЂРѕР·СЂР°С…СѓРЅРѕРє.</param>
    public static void EnsureRequestValid(LoanTotalRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Reader);
        EnsureHeaderValid(
            request.LoanId, request.Reader.Name, request.Reader.Email);
        EnsureLinesValid(request.Lines);
        ArgumentOutOfRangeException.ThrowIfNegative(request.DeliveryPrice);
    }
}
