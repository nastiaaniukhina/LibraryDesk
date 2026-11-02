namespace LibraryDesk.Core.Domain;

/// <summary>Читач бібліотеки.</summary>
public sealed class Reader
{
    /// <summary>Унікальний номер читача.</summary>
    public int Id { get; set; }

    /// <summary>Прізвище та ім'я читача.</summary>
    public string Name { get; set; }

    /// <summary>Ознака того, що читач є студентом.</summary>
    public bool IsStudent { get; set; }
}
