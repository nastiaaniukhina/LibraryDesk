namespace LibraryDesk.Core.Storage;

/// <summary>Журнал аудиту у файлі: дописує рядок на кожну операцію.</summary>
public sealed class FileAuditLog : IDisposable
{
    private readonly StreamWriter _writer;
    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="FileAuditLog"/> class.</summary>
    /// <param name="path">Шлях до файлу журналу.</param>
    public FileAuditLog(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _writer = new StreamWriter(path, append: true);
    }

    /// <summary>Дописує рядок аудиту.</summary>
    /// <param name="operation">Назва операції.</param>
    /// <param name="documentId">Ідентифікатор видачі.</param>
    public void Write(string operation, int documentId)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        _writer.WriteLine($"{DateTimeOffset.Now:O};{operation};{documentId}");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _writer.Flush();
        _writer.Dispose();
        _disposed = true;
    }
}
