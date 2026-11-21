using LibraryDesk.Core.Abstractions;

namespace LibraryDesk.Core.Notifications;

/// <summary>РЎРїРѕРІС–С‰СѓРІР°С‡, С‰Рѕ РїРёС€Рµ РїРѕРІС–РґРѕРјР»РµРЅРЅСЏ РІ РїРµСЂРµРґР°РЅРёР№ РїРѕС‚С–Рє РІРёРІРѕРґСѓ.</summary>
public sealed class TextWriterNotifier : INotifier
{
    private readonly TextWriter _output;

    /// <summary>Initializes a new instance of the <see cref="TextWriterNotifier"/> class.</summary>
    /// <param name="output">РџРѕС‚С–Рє РІРёРІРѕРґСѓ.</param>
    public TextWriterNotifier(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <inheritdoc/>
    public void Notify(string recipient, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        _output.WriteLine($"Р›РёСЃС‚ РґР»СЏ {recipient}: {message}");
    }
}
