using LibraryDesk.App;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Notifications;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Services;
using LibraryDesk.Core.Storage;

Console.OutputEncoding = System.Text.Encoding.UTF8;
TimeProvider clock = TimeProvider.System;
string dataPath = Path.Combine("data", "library.json");
string logPath = Path.Combine("data", "scenario.log");
JsonLoanRepository repository = new(dataPath);
if (args.Length > 0 && args[0] == "seed")
{
    foreach (var loan in SampleDataGenerator.Generate()) repository.Add(loan);
    repository.Save();
    return;
}

IPricingCatalog pricing = PricingCatalog.Default();
TextWriterNotifier notifier = new(Console.Out);
IScenarioLog log = new FileScenarioLog(logPath, clock);
LoanLifecycleService lifecycle = new(repository, pricing, notifier, clock);
LoanQueryService queries = new(repository, clock, pricing);
ScenarioRunner runner = new(log, Console.Out);
ConsoleMenu menu = new(lifecycle, queries, runner, Console.In, Console.Out);
return menu.Run();