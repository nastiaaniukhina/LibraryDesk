using System.Diagnostics.CodeAnalysis;

namespace LibraryDesk.Core.Errors;

/// <summary>Результат очікуваної операції: значення або пояснення помилки.</summary>
/// <typeparam name="T">Тип значення.</typeparam>
[SuppressMessage(
    "Design",
    "CA1000:Do not declare static members on generic types",
    Justification = "Фабричні методи Ok/Fail є частиною API типу-результату за методичкою ЛР 5.")]
public readonly struct Result<T>
{
    private Result(bool isSuccess, T? value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>Gets a value indicating whether операція завершилась успішно.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets значення успішного результату.</summary>
    public T? Value { get; }

    /// <summary>Gets пояснення помилки; порожнє, якщо операція успішна.</summary>
    public string Error { get; }

    /// <summary>Створює успішний результат.</summary>
    /// <param name="value">Значення.</param>
    /// <returns>Успішний результат.</returns>
    public static Result<T> Ok(T value) => new(true, value, string.Empty);

    /// <summary>Створює результат із помилкою.</summary>
    /// <param name="error">Пояснення помилки.</param>
    /// <returns>Неуспішний результат.</returns>
    public static Result<T> Fail(string error) => new(false, default, error);
}
