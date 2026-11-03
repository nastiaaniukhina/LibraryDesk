namespace LibraryDesk.Core.Lending;

/// <summary>Дані читача, що потрібні для розрахунку.</summary>
/// <param name="Name">Прізвище читача.</param>
/// <param name="Email">Пошта читача.</param>
/// <param name="IsRegular">Чи є читач постійним.</param>
public sealed record ReaderInfo(string Name, string Email, bool IsRegular);
