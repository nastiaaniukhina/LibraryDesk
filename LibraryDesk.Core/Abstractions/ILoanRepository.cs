using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>Сховище видач.</summary>
public interface ILoanRepository
{
    /// <summary>Додає видачу до сховища.</summary>
    /// <param name="loan">Видача.</param>
    void Add(Loan loan);

    /// <summary>Шукає видачу за ідентифікатором.</summary>
    /// <param name="id">Ідентифікатор видачі.</param>
    /// <returns>Видача або null, якщо її немає.</returns>
    Loan? GetById(int id);

    /// <summary>Повертає всі видачі.</summary>
    /// <returns>Копія списку видач.</returns>
    IReadOnlyList<Loan> GetAll();
}
