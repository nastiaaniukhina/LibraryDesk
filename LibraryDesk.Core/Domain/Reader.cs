namespace LibraryDesk.Core.Domain;

/// <summary>Читач бібліотеки.</summary>
public sealed class Reader
{
    /// <summary>Gets or sets the reader's unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the reader's full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets a value indicating whether the reader is a student.</summary>
    public bool IsStudent { get; set; }
}
