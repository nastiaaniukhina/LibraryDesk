namespace LibraryDesk.Core.Lending;

/// <summary>Результат розрахунку підсумку видачі.</summary>
/// <param name="Total">Підсумок до сплати.</param>
/// <param name="Status">Стан видачі після розрахунку.</param>
public sealed record LoanTotalResult(decimal Total, LoanFormStatus Status);
