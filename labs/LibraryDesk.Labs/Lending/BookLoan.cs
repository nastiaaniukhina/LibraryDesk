namespace LibraryDesk.Labs.Lending;

/// <summary>
/// Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎР В° Р С”Р Р…Р С‘Р В¶Р С•Р С” РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР ВµР Р†РЎвЂ“: Р В·Р В°Р С–Р С•Р В»Р С•Р Р†Р С•Р С” Р Т‘Р С•Р С”РЎС“Р СР ВµР Р…РЎвЂљР В° РЎвЂљР В° Р в„–Р С•Р С–Р С• РЎР‚РЎРЏР Т‘Р С”Р С‘.
/// </summary>
public class BookLoan
{
    /// <summary>Р РЋРЎвЂљР В°Р Р… Р’В«Р В·Р В°Р В±РЎР‚Р С•Р Р…РЎРЉР С•Р Р†Р В°Р Р…Р С•Р’В» (Р С—Р С•РЎвЂЎР В°РЎвЂљР С”Р С•Р Р†Р С‘Р в„–).</summary>
    public const int StatusReserved = 0;

    /// <summary>Р РЋРЎвЂљР В°Р Р… Р’В«Р Р†Р С‘Р Т‘Р В°Р Р…Р С•Р’В».</summary>
    public const int StatusIssued = 1;

    /// <summary>Р РЋРЎвЂљР В°Р Р… Р’В«Р С—Р С•Р Р†Р ВµРЎР‚Р Р…Р ВµР Р…Р С•Р’В».</summary>
    public const int StatusReturned = 2;

    /// <summary>Р РЋРЎвЂљР В°Р Р… Р’В«РЎРѓР С”Р В°РЎРѓР С•Р Р†Р В°Р Р…Р С•Р’В».</summary>
    public const int StatusCancelled = 3;

    /// <summary>Р РЋРЎвЂљР В°Р Р†Р С”Р В° Р С—Р С•Р Т‘Р В°РЎвЂљР С”РЎС“ Р Р…Р В° Р Т‘Р С•Р Т‘Р В°Р Р…РЎС“ Р Р†Р В°РЎР‚РЎвЂљРЎвЂ“РЎРѓРЎвЂљРЎРЉ.</summary>
    private const decimal VatRate = 0.2m;

    /// <summary>Р СџР С•РЎР‚РЎвЂ“Р С– РЎРѓРЎС“Р СР С‘ Р Т‘Р В»РЎРЏ Р В·Р Р…Р С‘Р В¶Р С”Р С‘ Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С•Р СРЎС“ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎРЎС“.</summary>
    private const decimal RegularDiscountThreshold = 1000m;

    /// <summary>Р В§Р В°РЎРѓРЎвЂљР С”Р В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘ Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С•Р СРЎС“ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎРЎС“.</summary>
    private const decimal RegularDiscountRate = 0.10m;

    /// <summary>Р СџР С•РЎР‚РЎвЂ“Р С– РЎРѓРЎС“Р СР С‘ Р Т‘Р В»РЎРЏ Р В·Р Р…Р С‘Р В¶Р С”Р С‘ Р Р…Р В° Р Р†Р ВµР В»Р С‘Р С”РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎС“.</summary>
    private const decimal LargeOrderThreshold = 5000m;

    /// <summary>Р В§Р В°РЎРѓРЎвЂљР С”Р В° Р В·Р Р…Р С‘Р В¶Р С”Р С‘ Р Р…Р В° Р Р†Р ВµР В»Р С‘Р С”РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎС“.</summary>
    private const decimal LargeOrderDiscountRate = 0.15m;

    /// <summary>Р С™РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ РЎР‚РЎРЏР Т‘Р С”РЎвЂ“Р Р†, Р В· РЎРЏР С”Р С•РЎвЂ” Р Т‘РЎвЂ“РЎвЂќ Р С–РЎС“РЎР‚РЎвЂљР С•Р Р†Р В° Р В·Р Р…Р С‘Р В¶Р С”Р В°.</summary>
    private const int BulkLineCount = 10;

    /// <summary>Р РЋРЎС“Р СР В° Р С–РЎС“РЎР‚РЎвЂљР С•Р Р†Р С•РЎвЂ” Р В·Р Р…Р С‘Р В¶Р С”Р С‘, Р С–РЎР‚Р Р….</summary>
    private const decimal BulkDiscountAmount = 100m;

    /// <summary>Р СњР В°Р в„–Р В±РЎвЂ“Р В»РЎРЉРЎв‚¬Р В° Р Т‘Р С•Р С—РЎС“РЎРѓРЎвЂљР С‘Р СР В° Р С”РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ РЎР‚РЎРЏР Т‘Р С”РЎвЂ“Р Р† Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    private const int MaxLineCount = 100;

    /// <summary>Р СњР В°Р в„–Р С”Р С•РЎР‚Р С•РЎвЂљРЎв‚¬Р В° Р Т‘Р С•Р С—РЎС“РЎРѓРЎвЂљР С‘Р СР В° Р Т‘Р С•Р Р†Р В¶Р С‘Р Р…Р В° Р С—РЎР‚РЎвЂ“Р В·Р Р†Р С‘РЎвЂ°Р В° РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</summary>
    private const int MinReaderNameLength = 3;

    private readonly List<string[]> _lines = new List<string[]>();

    /// <summary>Initializes a new instance of the <see cref="BookLoan"/> class.</summary>
    /// <param name="id">Р СњР С•Р СР ВµРЎР‚ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
    /// <param name="readerName">Р СџРЎР‚РЎвЂ“Р В·Р Р†Р С‘РЎвЂ°Р Вµ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</param>
    /// <param name="createdAt">Р СљР С•Р СР ВµР Р…РЎвЂљ РЎРѓРЎвЂљР Р†Р С•РЎР‚Р ВµР Р…Р Р…РЎРЏ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
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

    /// <summary>Р РЃРЎС“Р С”Р В°РЎвЂќ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎС“ Р В·Р В° Р Р…Р С•Р СР ВµРЎР‚Р С•Р С.</summary>
    /// <param name="loans">Р РЋР С—Р С‘РЎРѓР С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎ, РЎС“ РЎРЏР С”Р С•Р СРЎС“ Р Р†Р С‘Р С”Р С•Р Р…РЎС“РЎвЂќРЎвЂљРЎРЉРЎРѓРЎРЏ Р С—Р С•РЎв‚¬РЎС“Р С”.</param>
    /// <param name="loanId">Р СњР С•Р СР ВµРЎР‚ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, РЎРЏР С”РЎС“ РЎв‚¬РЎС“Р С”Р В°РЎР‹РЎвЂљРЎРЉ.</param>
    /// <returns>Р вЂ”Р Р…Р В°Р в„–Р Т‘Р ВµР Р…Р В° Р Р†Р С‘Р Т‘Р В°РЎвЂЎР В° Р В°Р В±Р С• null, РЎРЏР С”РЎвЂ°Р С• РЎвЂљР В°Р С”Р С•РЎвЂ” Р Р…Р ВµР СР В°РЎвЂќ.</returns>
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

    /// <summary>Р вЂќР С•Р Т‘Р В°РЎвЂќ Р Т‘Р С• Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ РЎР‚РЎРЏР Т‘Р С•Р С” РЎвЂ“Р В· Р С—РЎР‚Р С‘Р СРЎвЂ“РЎР‚Р Р…Р С‘Р С”Р С•Р С Р С”Р Р…Р С‘Р В¶Р С”Р С‘.</summary>
    /// <param name="isbn">Р С™Р С•Р Т‘ Р С”Р Р…Р С‘Р В¶Р С”Р С‘ (ISBN).</param>
    /// <param name="quantity">Р С™РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ Р С•Р Т‘Р С‘Р Р…Р С‘РЎвЂ РЎРЉ.</param>
    /// <param name="unitPrice">Р В¦РЎвЂ“Р Р…Р В° Р В·Р В° Р С•Р Т‘Р С‘Р Р…Р С‘РЎвЂ РЎР‹, Р С–РЎР‚Р Р….</param>
    public void AddLine(string isbn, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = isbn;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();
        _lines.Add(line);
    }

    /// <summary>
    /// Р С›Р В±РЎвЂЎР С‘РЎРѓР В»РЎР‹РЎвЂќ Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С”Р С•Р Р†РЎС“ РЎРѓРЎС“Р СРЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р В· РЎС“РЎР‚Р В°РЎвЂ¦РЎС“Р Р†Р В°Р Р…Р Р…РЎРЏР С Р В·Р Р…Р С‘Р В¶Р С•Р С” РЎвЂ“ Р СџР вЂќР вЂ™.
    /// </summary>
    /// <param name="isRegularReader">Р С›Р В·Р Р…Р В°Р С”Р В° Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С•Р С–Р С• РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</param>
    /// <returns>Р РЋРЎС“Р СР В° Р Т‘Р С• РЎРѓР С—Р В»Р В°РЎвЂљР С‘, Р В·Р В°Р С•Р С”РЎР‚РЎС“Р С–Р В»Р ВµР Р…Р В° Р Т‘Р С• Р Т‘Р Р†Р С•РЎвЂ¦ Р В·Р Р…Р В°Р С”РЎвЂ“Р Р†.</returns>
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

        // Р вЂ”Р Р…Р С‘Р В¶Р С”Р С‘ Р Р…Р Вµ Р С—Р С•РЎвЂќР Т‘Р Р…РЎС“РЎР‹РЎвЂљРЎРЉРЎРѓРЎРЏ: Р Т‘РЎвЂ“РЎвЂќ Р В»Р С‘РЎв‚¬Р Вµ Р С—Р ВµРЎР‚РЎв‚¬Р В° Р В· Р С—РЎвЂ“Р Т‘РЎвЂ¦Р С•Р Т‘РЎРЏРЎвЂ°Р С‘РЎвЂ¦
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

        // Р СџР вЂќР вЂ™ Р Р…Р В°РЎР‚Р В°РЎвЂ¦Р С•Р Р†РЎС“РЎвЂќРЎвЂљРЎРЉРЎРѓРЎРЏ Р Р…Р В° РЎРѓРЎС“Р СРЎС“ Р Р†Р В¶Р Вµ Р С—РЎвЂ“РЎРѓР В»РЎРЏ Р Р†РЎРѓРЎвЂ“РЎвЂ¦ Р В·Р Р…Р С‘Р В¶Р С•Р С”
        total += total * VatRate;
        return Math.Round(total, 2);
    }

    /// <summary>
    /// Р вЂ”Р СРЎвЂ“Р Р…РЎР‹РЎвЂќ РЎРѓРЎвЂљР В°Р Р… Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, РЎРЏР С”РЎвЂ°Р С• Р С—Р ВµРЎР‚Р ВµРЎвЂ¦РЎвЂ“Р Т‘ Р Т‘Р С•Р В·Р Р†Р С•Р В»Р ВµР Р…Р С‘Р в„– Р С—РЎР‚Р В°Р Р†Р С‘Р В»Р В°Р СР С‘.
    /// </summary>
    /// <param name="newStatus">Р В¦РЎвЂ“Р В»РЎРЉР С•Р Р†Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р… Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
    /// <returns>
    /// true, РЎРЏР С”РЎвЂ°Р С• Р С—Р ВµРЎР‚Р ВµРЎвЂ¦РЎвЂ“Р Т‘ Р Р†Р С‘Р С”Р С•Р Р…Р В°Р Р…Р С•; false, РЎРЏР С”РЎвЂ°Р С• Р С—Р ВµРЎР‚Р ВµРЎвЂ¦РЎвЂ“Р Т‘ Р В·Р В°Р В±Р С•РЎР‚Р С•Р Р…Р ВµР Р…Р С‘Р в„– Р С—РЎР‚Р В°Р Р†Р С‘Р В»Р В°Р СР С‘.
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

    /// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚РЎРЏРЎвЂќ, РЎвЂЎР С‘ Р Р†Р С‘Р Т‘Р В°РЎвЂЎР В° Р В·Р В°Р С—Р С•Р Р†Р Р…Р ВµР Р…Р В° Р С”Р С•РЎР‚Р ВµР С”РЎвЂљР Р…Р С•.</summary>
    /// <returns>true, РЎРЏР С”РЎвЂ°Р С• Р Р†РЎРѓРЎвЂ“ Р С•Р В±Р С•Р Р†'РЎРЏР В·Р С”Р С•Р Р†РЎвЂ“ Р С—Р С•Р В»РЎРЏ Р В·Р В°Р С—Р С•Р Р†Р Р…Р ВµР Р…РЎвЂ“ Р С—РЎР‚Р В°Р Р†Р С‘Р В»РЎРЉР Р…Р С•.</returns>
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

    /// <summary>Р В¤Р С•РЎР‚Р СРЎС“РЎвЂќ РЎвЂљР ВµР С”РЎРѓРЎвЂљР С•Р Р†Р С‘Р в„– Р В·Р Р†РЎвЂ“РЎвЂљ Р С—РЎР‚Р С• Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎС“.</summary>
    /// <returns>Р вЂ”Р Р†РЎвЂ“РЎвЂљ: Р С—Р С• РЎР‚РЎРЏР Т‘Р С”РЎС“ Р Р…Р В° Р С”Р С•Р В¶Р ВµР Р… Р С—РЎР‚Р С‘Р СРЎвЂ“РЎР‚Р Р…Р С‘Р С” РЎвЂ“ Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С•Р С”.</returns>
    public string BuildReport()
    {
        string report = string.Empty;

        for (int i = 0; i < _lines.Count; i++)
        {
            report = report + "Р С™Р Р…Р С‘Р В¶Р С”Р В°: " + _lines[i][0]
                + "; Р С”РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ: " + _lines[i][1]
                + "; РЎвЂ РЎвЂ“Р Р…Р В°: " + _lines[i][2]
                + "; РЎРѓРЎС“Р СР В°: " + (int.Parse(_lines[i][1]) * decimal.Parse(_lines[i][2])) + "\n";
        }

        report = report + "Р В Р В°Р В·Р С•Р С: " + CalculateTotal(false) + "\n";
        return report;
    }
}
