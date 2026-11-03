namespace LibraryDesk.Core.Lending;

/// <summary>Вхідні дані розрахунку підсумку видачі.</summary>
/// <param name="LoanId">Номер видачі.</param>
/// <param name="Reader">Дані читача.</param>
/// <param name="Lines">Рядки видачі.</param>
/// <param name="CreatedAt">Дата створення видачі.</param>
/// <param name="CouponCode">Код купона або null.</param>
/// <param name="Status">Поточний стан видачі.</param>
/// <param name="DeliveryPrice">Вартість доставки.</param>
public sealed record LoanTotalRequest(
    int LoanId,
    ReaderInfo Reader,
    IReadOnlyList<LoanLine> Lines,
    DateOnly CreatedAt,
    string? CouponCode,
    LoanFormStatus Status,
    decimal DeliveryPrice);
