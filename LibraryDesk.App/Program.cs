using System.Globalization;
using LibraryDesk.Core.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Loan loan = new()
{
    Id = 1,
    ReaderId = 10,
    CreatedAt = DateTimeOffset.Now,
};

loan.AddItem(new LoanItem
{
    Isbn = "ISBN-001",
    Days = 14,
    DailyRate = 0.50m,
});
loan.AddItem(new LoanItem
{
    Isbn = "ISBN-002",
    Days = 7,
    DailyRate = 1.20m,
});

string total = loan.Total()
    .ToString("F2", CultureInfo.InvariantCulture);

Console.WriteLine($"Видача #{loan.Id}");
Console.WriteLine($"Стан: {loan.Status}");
Console.WriteLine($"Позицій: {loan.Items.Count}");
Console.WriteLine($"Сума: {total}");
