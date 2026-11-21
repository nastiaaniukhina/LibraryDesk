using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>Р“РµРЅРµСЂСѓС” РІС–РґС‚РІРѕСЂСЋРІР°РЅС– СѓРјРѕРІРЅС– РґР°РЅС– РґР»СЏ РґРµРјРѕРЅСЃС‚СЂР°С†С–С—.</summary>
public static class SampleDataGenerator
{
    /// <summary>РџРѕС‡Р°С‚РєРѕРІРµ Р·РЅР°С‡РµРЅРЅСЏ РіРµРЅРµСЂР°С‚РѕСЂР°.</summary>
    public const int SeedValue = 471;

    /// <summary>РЎС‚РІРѕСЂСЋС” СЃС‚Рѕ С‚РµСЃС‚РѕРІРёС… РІРёРґР°С‡ РґР»СЏ С‚СЂРёРґС†СЏС‚Рё СѓРјРѕРІРЅРёС… С‡РёС‚Р°С‡С–РІ.</summary>
    public static IReadOnlyList<Loan> Generate()
    {
        Random random = new(SeedValue);
        List<Loan> loans = new();
        DateTimeOffset start = new(2026, 1, 1, 9, 0, 0, TimeSpan.Zero);
        for (int i = 1; i <= 100; i++)
        {
            ReaderKind kind = (ReaderKind)(i % 3);
            Loan loan = new(i, (i % 30) + 1, start.AddDays(i), kind);
            loan.AddItem(new LoanItem($"ISBN-{i:000000}", random.Next(3, 15), 0.50m + (random.Next(0, 6) / 10m)));
            if (i % 4 != 0)
            {
                loan.Issue(start.AddDays(i));
            }

            if (i % 4 == 1)
            {
                loan.MarkOverdue(start.AddDays(i + 20));
            }

            loans.Add(loan);
        }

        return loans;
    }
}
