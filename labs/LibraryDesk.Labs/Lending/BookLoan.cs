namespace LibraryDesk.Labs.Lending;

/// <summary>
/// Р’РёРґР°С‡Р° РєРЅРёР¶РѕРє С‡РёС‚Р°С‡РµРІС–: Р·Р°РіРѕР»РѕРІРѕРє РґРѕРєСѓРјРµРЅС‚Р° С‚Р° Р№РѕРіРѕ СЂСЏРґРєРё.
/// </summary>
public class BookLoan
{
    /// <summary>РЎС‚Р°РЅ В«Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅРѕВ» (РїРѕС‡Р°С‚РєРѕРІРёР№).</summary>
    public const int StatusReserved = 0;

    /// <summary>РЎС‚Р°РЅ В«РІРёРґР°РЅРѕВ».</summary>
    public const int StatusIssued = 1;

    /// <summary>РЎС‚Р°РЅ В«РїРѕРІРµСЂРЅРµРЅРѕВ».</summary>
    public const int StatusReturned = 2;

    /// <summary>РЎС‚Р°РЅ В«СЃРєР°СЃРѕРІР°РЅРѕВ».</summary>
    public const int StatusCancelled = 3;

    /// <summary>РЎС‚Р°РІРєР° РїРѕРґР°С‚РєСѓ РЅР° РґРѕРґР°РЅСѓ РІР°СЂС‚С–СЃС‚СЊ.</summary>
    private const decimal VatRate = 0.2m;

    /// <summary>РџРѕСЂС–Рі СЃСѓРјРё РґР»СЏ Р·РЅРёР¶РєРё РїРѕСЃС‚С–Р№РЅРѕРјСѓ С‡РёС‚Р°С‡Сѓ.</summary>
    private const decimal RegularDiscountThreshold = 1000m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РїРѕСЃС‚С–Р№РЅРѕРјСѓ С‡РёС‚Р°С‡Сѓ.</summary>
    private const decimal RegularDiscountRate = 0.10m;

    /// <summary>РџРѕСЂС–Рі СЃСѓРјРё РґР»СЏ Р·РЅРёР¶РєРё РЅР° РІРµР»РёРєСѓ РІРёРґР°С‡Сѓ.</summary>
    private const decimal LargeOrderThreshold = 5000m;

    /// <summary>Р§Р°СЃС‚РєР° Р·РЅРёР¶РєРё РЅР° РІРµР»РёРєСѓ РІРёРґР°С‡Сѓ.</summary>
    private const decimal LargeOrderDiscountRate = 0.15m;

    /// <summary>РљС–Р»СЊРєС–СЃС‚СЊ СЂСЏРґРєС–РІ, Р· СЏРєРѕС— РґС–С” РіСѓСЂС‚РѕРІР° Р·РЅРёР¶РєР°.</summary>
    private const int BulkLineCount = 10;

    /// <summary>РЎСѓРјР° РіСѓСЂС‚РѕРІРѕС— Р·РЅРёР¶РєРё, РіСЂРЅ.</summary>
    private const decimal BulkDiscountAmount = 100m;

    /// <summary>РќР°Р№Р±С–Р»СЊС€Р° РґРѕРїСѓСЃС‚РёРјР° РєС–Р»СЊРєС–СЃС‚СЊ СЂСЏРґРєС–РІ РІРёРґР°С‡С–.</summary>
    private const int MaxLineCount = 100;

    /// <summary>РќР°Р№РєРѕСЂРѕС‚С€Р° РґРѕРїСѓСЃС‚РёРјР° РґРѕРІР¶РёРЅР° РїСЂС–Р·РІРёС‰Р° С‡РёС‚Р°С‡Р°.</summary>
    private const int MinReaderNameLength = 3;

    private readonly List<string[]> _lines = new List<string[]>();

    /// <summary>Initializes a new instance of the <see cref="BookLoan"/> class.</summary>
    /// <param name="id">РќРѕРјРµСЂ РІРёРґР°С‡С–.</param>
    /// <param name="readerName">РџСЂС–Р·РІРёС‰Рµ С‡РёС‚Р°С‡Р°.</param>
    /// <param name="createdAt">РњРѕРјРµРЅС‚ СЃС‚РІРѕСЂРµРЅРЅСЏ РІРёРґР°С‡С–.</param>
    public BookLoan(string id, string readerName, DateTimeOffset createdAt)
    {
        Id = id;
        ReaderName = readerName;
        CreatedAt = createdAt;
    }

    /// <summary>Gets the loan number.</summary>
    public string Id { get; private set; }

    /// <summary>Gets the reader's surname.</summary>
    public string ReaderName { get; private set; }

    /// <summary>Gets the current loan status.</summary>
    public int Status { get; private set; }

    /// <summary>Gets the loan creation date and time.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>РЁСѓРєР°С” РІРёРґР°С‡Сѓ Р·Р° РЅРѕРјРµСЂРѕРј.</summary>
    /// <param name="loans">РЎРїРёСЃРѕРє РІРёРґР°С‡, Сѓ СЏРєРѕРјСѓ РІРёРєРѕРЅСѓС”С‚СЊСЃСЏ РїРѕС€СѓРє.</param>
    /// <param name="loanId">РќРѕРјРµСЂ РІРёРґР°С‡С–, СЏРєСѓ С€СѓРєР°СЋС‚СЊ.</param>
    /// <returns>Р—РЅР°Р№РґРµРЅР° РІРёРґР°С‡Р° Р°Р±Рѕ null, СЏРєС‰Рѕ С‚Р°РєРѕС— РЅРµРјР°С”.</returns>
    public static BookLoan? FindById(List<BookLoan> loans, string loanId)
    {
        for (int i = 0; i < loans.Count; i++)
        {
            if (loans[i].Id == loanId)
            {
                return loans[i];
            }
        }

        return null;
    }

    /// <summary>Р”РѕРґР°С” РґРѕ РІРёРґР°С‡С– СЂСЏРґРѕРє С–Р· РїСЂРёРјС–СЂРЅРёРєРѕРј РєРЅРёР¶РєРё.</summary>
    /// <param name="isbn">РљРѕРґ РєРЅРёР¶РєРё (ISBN).</param>
    /// <param name="quantity">РљС–Р»СЊРєС–СЃС‚СЊ РѕРґРёРЅРёС†СЊ.</param>
    /// <param name="unitPrice">Р¦С–РЅР° Р·Р° РѕРґРёРЅРёС†СЋ, РіСЂРЅ.</param>
    public void AddLine(string isbn, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = isbn;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();
        _lines.Add(line);
    }

    /// <summary>
    /// РћР±С‡РёСЃР»СЋС” РїС–РґСЃСѓРјРєРѕРІСѓ СЃСѓРјСѓ РІРёРґР°С‡С– Р· СѓСЂР°С…СѓРІР°РЅРЅСЏРј Р·РЅРёР¶РѕРє С– РџР”Р’.
    /// </summary>
    /// <param name="isRegularReader">РћР·РЅР°РєР° РїРѕСЃС‚С–Р№РЅРѕРіРѕ С‡РёС‚Р°С‡Р°.</param>
    /// <returns>РЎСѓРјР° РґРѕ СЃРїР»Р°С‚Рё, Р·Р°РѕРєСЂСѓРіР»РµРЅР° РґРѕ РґРІРѕС… Р·РЅР°РєС–РІ.</returns>
    public decimal CalculateTotal(bool isRegularReader)
    {
        decimal total = 0;
        int lineCount = 0;

        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(_lines[i][1]);
            decimal unitPrice = decimal.Parse(_lines[i][2]);
            total = total + (quantity * unitPrice);
            lineCount = lineCount + 1;
        }

        // Р—РЅРёР¶РєРё РЅРµ РїРѕС”РґРЅСѓСЋС‚СЊСЃСЏ: РґС–С” Р»РёС€Рµ РїРµСЂС€Р° Р· РїС–РґС…РѕРґСЏС‰РёС…
        if (isRegularReader && total > RegularDiscountThreshold)
        {
            total *= 1m - RegularDiscountRate;
        }
        else if (total > LargeOrderThreshold)
        {
            total *= 1m - LargeOrderDiscountRate;
        }

        if (lineCount > BulkLineCount)
        {
            total -= BulkDiscountAmount;
        }

        if (total < 0)
        {
            total = 0;
        }

        // РџР”Р’ РЅР°СЂР°С…РѕРІСѓС”С‚СЊСЃСЏ РЅР° СЃСѓРјСѓ РІР¶Рµ РїС–СЃР»СЏ РІСЃС–С… Р·РЅРёР¶РѕРє
        total += total * VatRate;
        return Math.Round(total, 2);
    }

    /// <summary>
    /// Р—РјС–РЅСЋС” СЃС‚Р°РЅ РІРёРґР°С‡С–, СЏРєС‰Рѕ РїРµСЂРµС…С–Рґ РґРѕР·РІРѕР»РµРЅРёР№ РїСЂР°РІРёР»Р°РјРё.
    /// </summary>
    /// <param name="newStatus">Р¦С–Р»СЊРѕРІРёР№ СЃС‚Р°РЅ РІРёРґР°С‡С–.</param>
    /// <returns>
    /// true, СЏРєС‰Рѕ РїРµСЂРµС…С–Рґ РІРёРєРѕРЅР°РЅРѕ; false, СЏРєС‰Рѕ РїРµСЂРµС…С–Рґ Р·Р°Р±РѕСЂРѕРЅРµРЅРёР№ РїСЂР°РІРёР»Р°РјРё.
    /// </returns>
    public bool TryChangeStatus(int newStatus)
    {
        if (Status == StatusReserved && newStatus == StatusIssued)
        {
            Status = StatusIssued;
            return true;
        }

        if (Status == StatusIssued && newStatus == StatusReturned)
        {
            Status = StatusReturned;
            return true;
        }

        if (Status == StatusReserved && newStatus == StatusCancelled)
        {
            Status = StatusCancelled;
            return true;
        }

        return false;
    }

    /// <summary>РџРµСЂРµРІС–СЂСЏС”, С‡Рё РІРёРґР°С‡Р° Р·Р°РїРѕРІРЅРµРЅР° РєРѕСЂРµРєС‚РЅРѕ.</summary>
    /// <returns>true, СЏРєС‰Рѕ РІСЃС– РѕР±РѕРІ'СЏР·РєРѕРІС– РїРѕР»СЏ Р·Р°РїРѕРІРЅРµРЅС– РїСЂР°РІРёР»СЊРЅРѕ.</returns>
    public bool IsValid()
    {
        if (Id is not null && Id.Length > 0
            && ReaderName is not null
            && ReaderName.Length >= MinReaderNameLength
            && _lines.Count > 0
            && _lines.Count < MaxLineCount
            && Status >= StatusReserved
            && Status <= StatusCancelled)
        {
            return true;
        }

        return false;
    }

    /// <summary>Р¤РѕСЂРјСѓС” С‚РµРєСЃС‚РѕРІРёР№ Р·РІС–С‚ РїСЂРѕ РІРёРґР°С‡Сѓ.</summary>
    /// <returns>Р—РІС–С‚: РїРѕ СЂСЏРґРєСѓ РЅР° РєРѕР¶РµРЅ РїСЂРёРјС–СЂРЅРёРє С– РїС–РґСЃСѓРјРѕРє.</returns>
    public string BuildReport()
    {
        string report = string.Empty;

        for (int i = 0; i < _lines.Count; i++)
        {
            report = report + "РљРЅРёР¶РєР°: " + _lines[i][0]
                + "; РєС–Р»СЊРєС–СЃС‚СЊ: " + _lines[i][1]
                + "; С†С–РЅР°: " + _lines[i][2]
                + "; СЃСѓРјР°: " + (int.Parse(_lines[i][1]) * decimal.Parse(_lines[i][2])) + "\n";
        }

        report = report + "Р Р°Р·РѕРј: " + CalculateTotal(false) + "\n";
        return report;
    }
}
