using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Services;

/// <summary>Запускає сценарії та централізує обробку очікуваних помилок.</summary>
public sealed class ScenarioRunner
{
    private readonly IScenarioLog _log;
    private readonly TextWriter _output;

    /// <summary>Створює виконавець сценаріїв.</summary>
    public ScenarioRunner(IScenarioLog log, TextWriter output)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    /// <summary>Виконує сценарій і обробляє доменні та файлові помилки.</summary>
    public void Run(string scenario, Action action)
    {
        _log.Information(scenario, "Початок");
        try
        {
            action();
            _log.Information(scenario, "Завершено");
        }
        catch (DomainRuleException exception)
        {
            _log.Warning(scenario, exception.Message);
            _output.WriteLine(exception.Message);
        }
        catch (IOException exception)
        {
            _log.Error(scenario, exception.Message);
            _output.WriteLine("Помилка роботи з файлом.");
        }
    }
}