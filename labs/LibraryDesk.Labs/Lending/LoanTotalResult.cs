namespace LibraryDesk.Labs.Lending;

/// <summary>Р В Р ВµР В·РЎС“Р В»РЎРЉРЎвЂљР В°РЎвЂљ РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С”РЎС“ Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С”РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
/// <param name="Total">Р СџРЎвЂ“Р Т‘РЎРѓРЎС“Р СР С•Р С” Р Т‘Р С• РЎРѓР С—Р В»Р В°РЎвЂљР С‘.</param>
/// <param name="Status">Р РЋРЎвЂљР В°Р Р… Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“ Р С—РЎвЂ“РЎРѓР В»РЎРЏ РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С”РЎС“.</param>
public sealed record LoanTotalResult(decimal Total, LoanFormStatus Status);
