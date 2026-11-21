namespace LibraryDesk.Core.Abstractions;

/// <summary>Журнал сценаріїв застосунку.</summary>
public interface IScenarioLog
{
    /// <summary>Записує інформаційне повідомлення.</summary>
    void Information(string scenario, string message);
    /// <summary>Записує попередження.</summary>
    void Warning(string scenario, string message);
    /// <summary>Записує помилку.</summary>
    void Error(string scenario, string message);
}