using LibraryDesk.Core.Abstractions;

namespace LibraryDesk.Core.Notifications;

/// <summary>Сповіщувач, що пише повідомлення в переданий потік виводу.</summary>
public sealed class TextWriterNotifier : INotifier
{
    private readonly TextWriter _output;

    /// <summary>Initializes a new instance of the <see cref="TextWriterNotifier"/> class.</summary>
    /// <param name="output">Потік виводу.</param>
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
        _output.WriteLine($"Лист для {recipient}: {message}");
    }
}
