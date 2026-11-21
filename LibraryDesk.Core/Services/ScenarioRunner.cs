using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Services;

/// <summary>Р—Р°РїСѓСЃРєР°С” СЃС†РµРЅР°СЂС–С— С‚Р° С†РµРЅС‚СЂР°Р»С–Р·СѓС” РѕР±СЂРѕР±РєСѓ РѕС‡С–РєСѓРІР°РЅРёС… РїРѕРјРёР»РѕРє.</summary>
public sealed class ScenarioRunner
{
    private readonly IScenarioLog _log;
    private readonly TextWriter _output;

    /// <summary>РЎС‚РІРѕСЂСЋС” РІРёРєРѕРЅР°РІРµС†СЊ СЃС†РµРЅР°СЂС–С—РІ.</summary>
    public ScenarioRunner(IScenarioLog log, TextWriter output)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    /// <summary>Р’РёРєРѕРЅСѓС” СЃС†РµРЅР°СЂС–Р№ С– РѕР±СЂРѕР±Р»СЏС” РґРѕРјРµРЅРЅС– С‚Р° С„Р°Р№Р»РѕРІС– РїРѕРјРёР»РєРё.</summary>
    public void Run(string scenario, Action action)
    {
        _log.Information(scenario, "РџРѕС‡Р°С‚РѕРє");
        try
        {
            action();
            _log.Information(scenario, "Р—Р°РІРµСЂС€РµРЅРѕ");
        }
        catch (DomainRuleException exception)
        {
            _log.Warning(scenario, exception.Message);
            _output.WriteLine(exception.Message);
        }
        catch (IOException exception)
        {
            _log.LogError(scenario, exception.Message);
            _output.WriteLine("РџРѕРјРёР»РєР° СЂРѕР±РѕС‚Рё Р· С„Р°Р№Р»РѕРј.");
        }
    }
}
