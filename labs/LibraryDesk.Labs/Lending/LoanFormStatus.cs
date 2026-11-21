namespace LibraryDesk.Labs.Lending;

/// <summary>Р РЋРЎвЂљР В°Р Р… РЎвЂћР С•РЎР‚Р СРЎС“Р В»РЎРЏРЎР‚Р В° Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
public enum LoanFormStatus
{
    /// <summary>Р С™Р Р…Р С‘Р В¶Р С”РЎС“ Р В·Р В°Р В±РЎР‚Р С•Р Р…РЎРЉР С•Р Р†Р В°Р Р…Р С•.</summary>
    Reserved,

    /// <summary>Р С™Р Р…Р С‘Р В¶Р С”РЎС“ Р Р†Р С‘Р Т‘Р В°Р Р…Р С• РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎРЎС“.</summary>
    Issued,

    /// <summary>Р С™Р Р…Р С‘Р В¶Р С”РЎС“ Р С—Р С•Р Р†Р ВµРЎР‚Р Р…Р ВµР Р…Р С•.</summary>
    Returned,

    /// <summary>Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎРЎС“ РЎРѓР С”Р В°РЎРѓР С•Р Р†Р В°Р Р…Р С•.</summary>
    Cancelled,
}
