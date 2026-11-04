using System.Globalization;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Notifications;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Reports;
using LibraryDesk.Core.Services;
using LibraryDesk.Core.Storage;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Композиційний корінь: єдине місце, де створюються конкретні реалізації.
string readerType = args.Length > 0 ? args[0] : "student";

Dictionary<string, IFeePolicy> policies = new()
{
    ["regular"] = new DiscountFeePolicy(0.05m),
    ["student"] = new StudentFeePolicy(),
    ["staff"] = new DiscountFeePolicy(0.30m),
};

IFeePolicy policy = policies.TryGetValue(readerType, out IFeePolicy? found)
    ? found
    : new StandardFeePolicy();

InMemoryLoanRepository repository = new();
TextWriterNotifier notifier = new(Console.Out);
LoanService service = new(repository, policy, notifier);

// Сценарій
DateTimeOffset now = DateTimeOffset.Now;
Reader reader = new(10, "Іваненко Іван", "ivan@example.com") { IsStudent = true };
Loan loan = new(1, reader.Id, now);
loan.AddItem(new LoanItem("ISBN-001", 14, 0.50m));
loan.AddItem(new LoanItem("ISBN-002", 7, 1.20m));
service.Place(loan, reader);
loan.Issue(now);

string total = service.TotalOf(loan).ToString("F2", CultureInfo.InvariantCulture);
Console.WriteLine($"Видача #{loan.Id}, політика: {readerType}");
Console.WriteLine($"Стан: {loan.Status}");
Console.WriteLine($"Сума: {total}");

loan.Return();
ArchivedLoan archived = new(loan, now);
Console.WriteLine($"Архів: видача #{archived.Id}, стан {archived.Status}");
Console.Write(LoanCsvReport.BuildCsv(repository.GetAll(), service.TotalOf));
