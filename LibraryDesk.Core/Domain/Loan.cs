namespace LibraryDesk.Core.Domain;

/// <summary>Р’РёРґР°С‡Р° РєРЅРёР¶РѕРє С‡РёС‚Р°С‡РµРІС–: Р°РіСЂРµРіР°С‚ С–Р· РєРѕРЅС‚СЂРѕР»СЊРѕРІР°РЅРёРјРё РїРµСЂРµС…РѕРґР°РјРё СЃС‚Р°РЅС–РІ.</summary>
public sealed class Loan
{
    private readonly List<LoanItem> _items = new();

    /// <summary>Initializes a new instance of the <see cref="Loan"/> class.</summary>
    /// <param name="id">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ РІРёРґР°С‡С–, Р±С–Р»СЊС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    /// <param name="readerId">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ С‡РёС‚Р°С‡Р°, Р±С–Р»СЊС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    /// <param name="createdAt">Р§Р°СЃ СЃС‚РІРѕСЂРµРЅРЅСЏ РІРёРґР°С‡С–.</param>
    public Loan(int id, int readerId, DateTimeOffset createdAt, ReaderKind readerKind = ReaderKind.Regular)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(readerId);

        Id = id;
        ReaderId = readerId;
        CreatedAt = createdAt;
        ReaderKind = readerKind;
        Status = LoanStatus.Reserved;
    }

    /// <summary>Gets С–РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ РІРёРґР°С‡С–.</summary>
    public int Id { get; }

    /// <summary>Gets С–РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ С‡РёС‚Р°С‡Р°.</summary>
    public int ReaderId { get; }

    /// <summary>Gets С‡Р°СЃ СЃС‚РІРѕСЂРµРЅРЅСЏ РІРёРґР°С‡С–.</summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>Gets РєР°С‚РµРіРѕСЂС–СЋ С‡РёС‚Р°С‡Р°.</summary>
    public ReaderKind ReaderKind { get; private set; }

    /// <summary>Gets РїРѕС‚РѕС‡РЅРёР№ СЃС‚Р°РЅ РІРёРґР°С‡С–.</summary>
    public LoanStatus Status { get; private set; }

    /// <summary>Gets РєС–РЅС†РµРІРёР№ СЃС‚СЂРѕРє РїРѕРІРµСЂРЅРµРЅРЅСЏ; null, РїРѕРєРё РєРЅРёР¶РєРё РЅРµ РІРёРґР°РЅРѕ.</summary>
    public DateTimeOffset? DueAt { get; private set; }

    /// <summary>Gets РјРѕРјРµРЅС‚ РїРѕРІРµСЂРЅРµРЅРЅСЏ; null, РїРѕРєРё РєРЅРёР¶РєРё РЅРµ РїРѕРІРµСЂРЅСѓС‚Рѕ.</summary>
    public DateTimeOffset? ReturnedAt { get; private set; }

    /// <summary>Gets РїРѕР·РёС†С–С— РІРёРґР°С‡С– Р»РёС€Рµ РґР»СЏ С‡РёС‚Р°РЅРЅСЏ.</summary>
    public IReadOnlyList<LoanItem> Items => _items.AsReadOnly();

    /// <summary>Р”РѕРґР°С” РїРѕР·РёС†С–СЋ РґРѕ РІРёРґР°С‡С– РІ СЃС‚Р°РЅС– Reserved.</summary>
    /// <param name="item">РџРѕР·РёС†С–СЏ РІРёРґР°С‡С–.</param>
    public void AddItem(LoanItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException(
                "Р”РѕРґР°РІР°С‚Рё РєРЅРёР¶РєРё РјРѕР¶РЅР° Р»РёС€Рµ РґРѕ Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅРѕС— РІРёРґР°С‡С–.");
        }

        _items.Add(item);
    }

    /// <summary>РџРµСЂРµРІРѕРґРёС‚СЊ РІРёРґР°С‡Сѓ Р·С– СЃС‚Р°РЅСѓ Reserved Сѓ Issued.</summary>
    /// <param name="now">РњРѕРјРµРЅС‚ РІРёРґР°С‡С– РєРЅРёР¶РѕРє.</param>
    public void Issue(DateTimeOffset now)
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException("Р’РёРґР°С‚Рё РјРѕР¶РЅР° Р»РёС€Рµ Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅСѓ РІРёРґР°С‡Сѓ.");
        }

        if (_items.Count == 0)
        {
            throw new InvalidLoanStateException("Р’РёРґР°С‡Р° РЅРµ РјС–СЃС‚РёС‚СЊ Р¶РѕРґРЅРѕС— РєРЅРёР¶РєРё.");
        }

        DueAt = now.AddDays(_items.Max(item => item.Days));
        Status = LoanStatus.Issued;
    }

    /// <summary>РџРµСЂРµРІРѕРґРёС‚СЊ РІРёРґР°С‡Сѓ Р·С– СЃС‚Р°РЅСѓ Issued Сѓ Overdue, СЏРєС‰Рѕ СЃС‚СЂРѕРє РјРёРЅСѓРІ.</summary>
    /// <param name="now">РџРѕС‚РѕС‡РЅРёР№ РјРѕРјРµРЅС‚ С‡Р°СЃСѓ.</param>
    public void MarkOverdue(DateTimeOffset now)
    {
        if (Status != LoanStatus.Issued)
        {
            throw new InvalidLoanStateException("РџСЂРѕСЃС‚СЂРѕС‡РёС‚Рё РјРѕР¶РЅР° Р»РёС€Рµ РІРёРґР°РЅСѓ РІРёРґР°С‡Сѓ.");
        }

        if (DueAt is not DateTimeOffset due || now < due)
        {
            throw new InvalidLoanStateException("РЎС‚СЂРѕРє РїРѕРІРµСЂРЅРµРЅРЅСЏ С‰Рµ РЅРµ РјРёРЅСѓРІ.");
        }

        Status = LoanStatus.Overdue;
    }

    /// <summary>РћР±С‡РёСЃР»СЋС” РєС–Р»СЊРєС–СЃС‚СЊ РєР°Р»РµРЅРґР°СЂРЅРёС… РґРЅС–РІ РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ РЅР° РІРєР°Р·Р°РЅСѓ РґР°С‚Сѓ.</summary>
    /// <param name="asOf">Р”Р°С‚Р°, РЅР° СЏРєСѓ РѕР±С‡РёСЃР»СЋС”С‚СЊСЃСЏ РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ.</param>
    /// <returns>РљС–Р»СЊРєС–СЃС‚СЊ РґРЅС–РІ РїСЂРѕСЃС‚СЂРѕС‡РµРЅРЅСЏ, Р°Р»Рµ РЅРµ РјРµРЅС€Рµ РЅСѓР»СЏ.</returns>
    public int OverdueDaysAt(DateOnly asOf)
    {
        if (DueAt is not DateTimeOffset dueAt)
        {
            return 0;
        }

        DateOnly dueDate = DateOnly.FromDateTime(dueAt.DateTime);
        return Math.Max(asOf.DayNumber - dueDate.DayNumber, 0);
    }

    /// <summary>РџРѕРІРµСЂС‚Р°С” РІРёРґР°С‡Сѓ Р·С– СЃС‚Р°РЅС–РІ Issued Р°Р±Рѕ Overdue.</summary>
    /// <param name="now">РњРѕРјРµРЅС‚ РїРѕРІРµСЂРЅРµРЅРЅСЏ РєРЅРёР¶РѕРє.</param>
    public void Return(DateTimeOffset now)
    {
        if (Status is not (LoanStatus.Issued or LoanStatus.Overdue))
        {
            throw new InvalidLoanStateException(
                "РџРѕРІРµСЂРЅСѓС‚Рё РјРѕР¶РЅР° Р»РёС€Рµ РІРёРґР°РЅСѓ Р°Р±Рѕ РїСЂРѕСЃС‚СЂРѕС‡РµРЅСѓ РІРёРґР°С‡Сѓ.");
        }

        ReturnedAt = now;
        Status = LoanStatus.Returned;
    }

    /// <summary>Р’С–РґРЅРѕРІР»СЋС” РІРёРґР°С‡Сѓ Р·С– Р·Р±РµСЂРµР¶РµРЅРѕРіРѕ Р·Р°РїРёСЃСѓ.</summary>
    /// <param name="id">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ РІРёРґР°С‡С–.</param>
    /// <param name="readerId">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ С‡РёС‚Р°С‡Р°.</param>
    /// <param name="createdAt">Р§Р°СЃ СЃС‚РІРѕСЂРµРЅРЅСЏ.</param>
    /// <param name="kind">РљР°С‚РµРіРѕСЂС–СЏ С‡РёС‚Р°С‡Р°.</param>
    /// <param name="status">Р—Р±РµСЂРµР¶РµРЅРёР№ СЃС‚Р°РЅ.</param>
    /// <param name="dueAt">РЎС‚СЂРѕРє РїРѕРІРµСЂРЅРµРЅРЅСЏ.</param>
    /// <param name="returnedAt">Р§Р°СЃ РїРѕРІРµСЂРЅРµРЅРЅСЏ.</param>
    /// <param name="items">РџРѕР·РёС†С–С— РІРёРґР°С‡С–.</param>
    /// <returns>Р’С–РґРЅРѕРІР»РµРЅР° РІРёРґР°С‡Р°.</returns>
    internal static Loan Restore(
        int id,
        int readerId,
        DateTimeOffset createdAt,
        ReaderKind kind,
        LoanStatus status,
        DateTimeOffset? dueAt,
        DateTimeOffset? returnedAt,
        IEnumerable<LoanItem> items)
    {
        Loan loan = new(id, readerId, createdAt, kind);
        foreach (LoanItem item in items)
        {
            loan._items.Add(item);
        }

        loan.Status = status;
        loan.DueAt = dueAt;
        loan.ReturnedAt = returnedAt;
        return loan;
    }

    /// <summary>РЎРєР°СЃРѕРІСѓС” Р±СЂРѕРЅСЊ, РїРѕРєРё РєРЅРёР¶РєРё РЅРµ РІРёРґР°РЅРѕ.</summary>
    public void Cancel()
    {
        if (Status != LoanStatus.Reserved)
        {
            throw new InvalidLoanStateException("РЎРєР°СЃСѓРІР°С‚Рё РјРѕР¶РЅР° Р»РёС€Рµ Р·Р°Р±СЂРѕРЅСЊРѕРІР°РЅСѓ РІРёРґР°С‡Сѓ.");
        }

        Status = LoanStatus.Cancelled;
    }
}
