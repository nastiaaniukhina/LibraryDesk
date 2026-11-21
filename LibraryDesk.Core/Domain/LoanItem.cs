namespace LibraryDesk.Core.Domain;

/// <summary>РќРµР·РјС–РЅРЅР° РїРѕР·РёС†С–СЏ РІРёРґР°С‡С–: РєРЅРёР¶РєР°, СЃС‚СЂРѕРє Сѓ РґРЅСЏС… С– РґРµРЅРЅРёР№ С‚Р°СЂРёС„.</summary>
public sealed record LoanItem
{
    /// <summary>Initializes a new instance of the <see cref="LoanItem"/> class.</summary>
    /// <param name="isbn">ISBN-РєРѕРґ РєРЅРёР¶РєРё, РЅРµ РїРѕСЂРѕР¶РЅС–Р№.</param>
    /// <param name="days">РЎС‚СЂРѕРє Сѓ РґРЅСЏС…, Р±С–Р»СЊС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    /// <param name="dailyRate">Р”РµРЅРЅРёР№ С‚Р°СЂРёС„, РЅРµ РјРµРЅС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    public LoanItem(string isbn, int days, decimal dailyRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(days);
        ArgumentOutOfRangeException.ThrowIfNegative(dailyRate);

        Isbn = isbn;
        Days = days;
        DailyRate = dailyRate;
    }

    /// <summary>Gets ISBN-РєРѕРґ РєРЅРёР¶РєРё.</summary>
    public string Isbn { get; }

    /// <summary>Gets СЃС‚СЂРѕРє РІРёРґР°С‡С– РІ РґРЅСЏС….</summary>
    public int Days { get; }

    /// <summary>Gets РґРµРЅРЅРёР№ С‚Р°СЂРёС„.</summary>
    public decimal DailyRate { get; }

    /// <summary>Gets РІР°СЂС‚С–СЃС‚СЊ РїРѕР·РёС†С–С— (РѕР±С‡РёСЃР»СЋС”С‚СЊСЃСЏ, Р° РЅРµ Р·Р±РµСЂС–РіР°С”С‚СЊСЃСЏ).</summary>
    public decimal Amount => Days * DailyRate;
}
