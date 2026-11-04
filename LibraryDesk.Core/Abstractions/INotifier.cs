namespace LibraryDesk.Core.Abstractions;

/// <summary>Сповіщувач про події видачі.</summary>
public interface INotifier
{
    /// <summary>Надсилає повідомлення отримувачу.</summary>
    /// <param name="recipient">Адреса отримувача.</param>
    /// <param name="message">Текст повідомлення.</param>
    void Notify(string recipient, string message);
}
