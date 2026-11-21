namespace LibraryDesk.Labs.Lending;

/// <summary>Р В РЎРЏР Т‘Р С•Р С” РЎвЂћР С•РЎР‚Р СРЎС“Р В»РЎРЏРЎР‚Р В° Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“: Р С”Р Р…Р С‘Р В¶Р С”Р В°, Р С”РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ Р С—РЎР‚Р С‘Р СРЎвЂ“РЎР‚Р Р…Р С‘Р С”РЎвЂ“Р Р† РЎвЂ“ РЎвЂ РЎвЂ“Р Р…Р В°.</summary>
public sealed class LoanLine
{
    /// <summary>Gets iSBN-Р С”Р С•Р Т‘ Р С”Р Р…Р С‘Р В¶Р С”Р С‘.</summary>
    public string Isbn { get; init; } = string.Empty;

    /// <summary>Gets Р С”РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ Р С—РЎР‚Р С‘Р СРЎвЂ“РЎР‚Р Р…Р С‘Р С”РЎвЂ“Р Р†.</summary>
    public int Quantity { get; init; }

    /// <summary>Gets РЎвЂ РЎвЂ“Р Р…РЎС“ Р С•Р Т‘Р Р…Р С•Р С–Р С• Р С—РЎР‚Р С‘Р СРЎвЂ“РЎР‚Р Р…Р С‘Р С”Р В°, Р С–РЎР‚Р Р….</summary>
    public decimal UnitPrice { get; init; }
}
