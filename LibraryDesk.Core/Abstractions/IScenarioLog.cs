namespace LibraryDesk.Core.Abstractions;

/// <summary>Р–СѓСЂРЅР°Р» СЃС†РµРЅР°СЂС–С—РІ Р·Р°СЃС‚РѕСЃСѓРЅРєСѓ.</summary>
public interface IScenarioLog
{
    /// <summary>Р—Р°РїРёСЃСѓС” С–РЅС„РѕСЂРјР°С†С–Р№РЅРµ РїРѕРІС–РґРѕРјР»РµРЅРЅСЏ.</summary>
    void Information(string scenario, string message);
    /// <summary>Р—Р°РїРёСЃСѓС” РїРѕРїРµСЂРµРґР¶РµРЅРЅСЏ.</summary>
    void Warning(string scenario, string message);
    /// <summary>Р—Р°РїРёСЃСѓС” РїРѕРјРёР»РєСѓ.</summary>
    void LogError(string scenario, string message);
}
