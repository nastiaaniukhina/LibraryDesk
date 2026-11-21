using LibraryDesk.Core.Abstractions;

namespace LibraryDesk.Core.Storage;

/// <summary>Р¤Р°Р№Р»РѕРІРёР№ Р¶СѓСЂРЅР°Р» СЃС†РµРЅР°СЂС–С—РІ.</summary>
public sealed class FileScenarioLog : IScenarioLog
{
    private readonly string _path;
    private readonly TimeProvider _time;

    /// <summary>РЎС‚РІРѕСЂСЋС” С„Р°Р№Р»РѕРІРёР№ Р¶СѓСЂРЅР°Р».</summary>
    public FileScenarioLog(string path, TimeProvider time)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
        _time = time ?? throw new ArgumentNullException(nameof(time));
    }

    /// <inheritdoc/>
    public void Information(string scenario, string message) => Write("Information", scenario, message);
    /// <inheritdoc/>
    public void Warning(string scenario, string message) => Write("Warning", scenario, message);
    /// <inheritdoc/>
    public void LogError(string scenario, string message) => Write("Error", scenario, message);

    private void Write(string level, string scenario, string message)
    {
        string line = $"{_time.GetUtcNow():O}|{level}|{scenario}|{message}{Environment.NewLine}";
        File.AppendAllText(_path, line);
    }
}
