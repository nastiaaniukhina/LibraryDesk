namespace LibraryDesk.Core.Domain;

/// <summary>Незмінний читач бібліотеки.</summary>
public sealed record Reader
{
    /// <summary>Initializes a new instance of the <see cref="Reader"/> class.</summary>
    /// <param name="id">Ідентифікатор читача, більший за нуль.</param>
    /// <param name="name">Прізвище та ім'я, не порожнє.</param>
    /// <param name="email">Адреса пошти, що містить символ @.</param>
    public Reader(int id, string name, string email)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if (!email.Contains('@'))
        {
            throw new ArgumentException("Пошта має містити символ @.", nameof(email));
        }

        Id = id;
        Name = name;
        Email = email;
    }

    /// <summary>Gets ідентифікатор читача.</summary>
    public int Id { get; }

    /// <summary>Gets прізвище та ім'я читача.</summary>
    public string Name { get; }

    /// <summary>Gets адресу пошти читача.</summary>
    public string Email { get; }

    /// <summary>Gets a value indicating whether читач є студентом.</summary>
    public bool IsStudent { get; init; }
}
