namespace LibraryDesk.Core.Domain;

/// <summary>РљРЅРёР¶РєР° РІ РєР°С‚Р°Р»РѕР·С–: Р·РјС–РЅСЋС”С‚СЊСЃСЏ Р»РёС€Рµ РєС–Р»СЊРєС–СЃС‚СЊ РІС–Р»СЊРЅРёС… РїСЂРёРјС–СЂРЅРёРєС–РІ.</summary>
public sealed class Book
{
    /// <summary>Initializes a new instance of the <see cref="Book"/> class.</summary>
    /// <param name="isbn">ISBN-РєРѕРґ, РЅРµ РїРѕСЂРѕР¶РЅС–Р№.</param>
    /// <param name="title">РќР°Р·РІР°, РЅРµ РїРѕСЂРѕР¶РЅСЏ.</param>
    /// <param name="author">РђРІС‚РѕСЂ, РЅРµ РїРѕСЂРѕР¶РЅС–Р№.</param>
    /// <param name="dailyRate">Р”РµРЅРЅРёР№ С‚Р°СЂРёС„, РЅРµ РјРµРЅС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    /// <param name="copiesAvailable">РљС–Р»СЊРєС–СЃС‚СЊ РІС–Р»СЊРЅРёС… РїСЂРёРјС–СЂРЅРёРєС–РІ, РЅРµ РјРµРЅС€Р° Р·Р° РЅСѓР»СЊ.</param>
    public Book(string isbn, string title, string author, decimal dailyRate, int copiesAvailable)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(author);
        ArgumentOutOfRangeException.ThrowIfNegative(dailyRate);
        ArgumentOutOfRangeException.ThrowIfNegative(copiesAvailable);

        Isbn = isbn;
        Title = title;
        Author = author;
        DailyRate = dailyRate;
        CopiesAvailable = copiesAvailable;
    }

    /// <summary>Gets ISBN-РєРѕРґ РєРЅРёР¶РєРё.</summary>
    public string Isbn { get; }

    /// <summary>Gets РЅР°Р·РІСѓ РєРЅРёР¶РєРё.</summary>
    public string Title { get; }

    /// <summary>Gets Р°РІС‚РѕСЂР° РєРЅРёР¶РєРё.</summary>
    public string Author { get; }

    /// <summary>Gets РґРµРЅРЅРёР№ С‚Р°СЂРёС„.</summary>
    public decimal DailyRate { get; }

    /// <summary>Gets РєС–Р»СЊРєС–СЃС‚СЊ РІС–Р»СЊРЅРёС… РїСЂРёРјС–СЂРЅРёРєС–РІ.</summary>
    public int CopiesAvailable { get; private set; }

    /// <summary>Р—Р°Р±РёСЂР°С” РѕРґРёРЅ РїСЂРёРјС–СЂРЅРёРє Р·С– СЃС…РѕРІРёС‰Р°.</summary>
    public void TakeCopy()
    {
        if (CopiesAvailable == 0)
        {
            throw new InvalidOperationException("Р’С–Р»СЊРЅРёС… РїСЂРёРјС–СЂРЅРёРєС–РІ РЅРµРјР°С”.");
        }

        CopiesAvailable--;
    }

    /// <summary>РџРѕРІРµСЂС‚Р°С” РѕРґРёРЅ РїСЂРёРјС–СЂРЅРёРє РґРѕ СЃС…РѕРІРёС‰Р°.</summary>
    public void ReturnCopy()
    {
        CopiesAvailable++;
    }
}
