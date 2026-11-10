using System.Globalization;
using LibraryDesk.App;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Errors;
using LibraryDesk.Core.Notifications;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Reports;
using LibraryDesk.Core.Services;
using LibraryDesk.Core.Storage;
using Microsoft.Extensions.Logging;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

using ILoggerFactory factory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.TimestampFormat = "HH:mm:ss ";
        options.IncludeScopes = true;
    });
    builder.SetMinimumLevel(LogLevel.Debug);
});

ILogger log = factory.CreateLogger("LibraryDesk.App");
Action<ILogger, string, string, Exception?> logWarning =
    LoggerMessage.Define<string, string>(
        LogLevel.Warning,
        new EventId(10, "DomainRuleWarning"),
        "Правило {Rule}: {Message}");

Action<ILogger, Exception?> logCritical =
    LoggerMessage.Define(
        LogLevel.Critical,
        new EventId(11, "UnexpectedFailure"),
        "Непередбачений збій");

// Верхній обробник: єдине місце, де дозволено catch (Exception).
try
{
    RunApplication(args, factory);
    return 0;
}
catch (DomainRuleException ex)
{
    logWarning(log, ex.Rule, ex.Message, ex);
    return 1;
}
catch (Exception ex)
{
    logCritical(log, ex);
    return 2;
}

static void RunApplication(string[] cmdArgs, ILoggerFactory factory)
{
    ILogger appLog = factory.CreateLogger("LibraryDesk.App");
    Action<ILogger, int, Exception?> logError =
    LoggerMessage.Define<int>(
        LogLevel.Error,
        new EventId(12, "ReturnLoanFailure"),
        "Не вдалося повернути видачу {LoanId}");

    // Композиційний корінь: єдине місце, де створюються конкретні реалізації.
    string readerType = cmdArgs.Length > 0 ? cmdArgs[0] : "student";

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
    LoanService service = new(repository, policy, notifier, factory.CreateLogger<LoanService>());

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

    // Демонстрація рівня Error: очікувана помилка предметної області
    try
    {
        service.ReturnLoan(999);
    }
    catch (DomainRuleException ex)
    {
        logError(appLog, 999, ex);
    }

    // Інтерактивне додавання позицій: розбір вводу живе зовні барикади
    Loan draft = new(2, reader.Id, now);
    service.Place(draft, reader);
    while (true)
    {
        Console.Write("ISBN (порожньо — завершити): ");
        string? isbn = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(isbn))
        {
            break;
        }

        Console.Write("Кількість днів: ");
        string? days = Console.ReadLine();
        Console.Write("Добова ставка (через крапку): ");
        string? rate = Console.ReadLine();

        Result<LoanItem> parsed = LoanItemParser.Parse(isbn, days, rate);
        if (!parsed.IsSuccess)
        {
            Console.WriteLine($"Помилка вводу: {parsed.Error}");
            continue;
        }

        service.AddItem(draft.Id, parsed.Value!);
        Console.WriteLine($"Додано. Позицій у видачі #{draft.Id}: {draft.Items.Count}");
    }
}
