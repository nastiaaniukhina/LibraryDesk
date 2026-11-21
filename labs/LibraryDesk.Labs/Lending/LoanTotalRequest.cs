namespace LibraryDesk.Labs.Lending;

/// <summary>Р вЂ™РЎвЂ¦РЎвЂ“Р Т‘Р Р…РЎвЂ“ Р Т‘Р В°Р Р…РЎвЂ“ РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С”РЎС“ Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С”РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
/// <param name="LoanId">Р СњР С•Р СР ВµРЎР‚ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
/// <param name="Reader">Р вЂќР В°Р Р…РЎвЂ“ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</param>
/// <param name="Lines">Р В РЎРЏР Т‘Р С”Р С‘ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
/// <param name="CreatedAt">Р вЂќР В°РЎвЂљР В° РЎРѓРЎвЂљР Р†Р С•РЎР‚Р ВµР Р…Р Р…РЎРЏ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
/// <param name="CouponCode">Р С™Р С•Р Т‘ Р С”РЎС“Р С—Р С•Р Р…Р В° Р В°Р В±Р С• null.</param>
/// <param name="Status">Р СџР С•РЎвЂљР С•РЎвЂЎР Р…Р С‘Р в„– РЎРѓРЎвЂљР В°Р Р… Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</param>
/// <param name="DeliveryPrice">Р вЂ™Р В°РЎР‚РЎвЂљРЎвЂ“РЎРѓРЎвЂљРЎРЉ Р Т‘Р С•РЎРѓРЎвЂљР В°Р Р†Р С”Р С‘.</param>
public sealed record LoanTotalRequest(
    int LoanId,
    ReaderInfo Reader,
    IReadOnlyList<LoanLine> Lines,
    DateOnly CreatedAt,
    string? CouponCode,
    LoanFormStatus Status,
    decimal DeliveryPrice);
