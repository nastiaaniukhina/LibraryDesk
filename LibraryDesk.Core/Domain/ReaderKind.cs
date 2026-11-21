namespace LibraryDesk.Core.Domain;

/// <summary>Категорія читача бібліотеки.</summary>
public enum ReaderKind
{
    /// <summary>Звичайний читач.</summary>
    Regular,
    /// <summary>Студент.</summary>
    Student,
    /// <summary>Працівник бібліотеки.</summary>
    Staff,
}