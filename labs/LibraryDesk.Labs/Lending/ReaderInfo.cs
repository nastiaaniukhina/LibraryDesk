namespace LibraryDesk.Labs.Lending;

/// <summary>Р вЂќР В°Р Р…РЎвЂ“ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°, РЎвЂ°Р С• Р С—Р С•РЎвЂљРЎР‚РЎвЂ“Р В±Р Р…РЎвЂ“ Р Т‘Р В»РЎРЏ РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С”РЎС“.</summary>
/// <param name="Name">Р СџРЎР‚РЎвЂ“Р В·Р Р†Р С‘РЎвЂ°Р Вµ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</param>
/// <param name="Email">Р СџР С•РЎв‚¬РЎвЂљР В° РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°.</param>
/// <param name="IsRegular">Р В§Р С‘ РЎвЂќ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎ Р С—Р С•РЎРѓРЎвЂљРЎвЂ“Р в„–Р Р…Р С‘Р С.</param>
public sealed record ReaderInfo(string Name, string Email, bool IsRegular);
