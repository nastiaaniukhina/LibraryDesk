namespace LibraryDesk.Labs.Lending;

/// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚Р С”Р С‘ Р С—Р ВµРЎР‚Р ВµР Т‘РЎС“Р СР С•Р Р† Р Т‘Р В»РЎРЏ Р Т‘Р В°Р Р…Р С‘РЎвЂ¦ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
public static class LoanGuards
{
    /// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚РЎРЏРЎвЂќ Р В·Р В°Р С–Р С•Р В»Р С•Р Р†Р С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“: Р Р…Р С•Р СР ВµРЎР‚, Р С—РЎР‚РЎвЂ“Р В·Р Р†Р С‘РЎвЂ°Р Вµ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°, Р С—Р С•РЎв‚¬РЎвЂљРЎС“.</summary>
    /// <param name="loanId">Р СњР С•Р СР ВµРЎР‚ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, Р СР В°РЎвЂќ Р В±РЎС“РЎвЂљР С‘ Р Т‘Р С•Р Т‘Р В°РЎвЂљР Р…Р С‘Р С.</param>
    /// <param name="readerName">Р СџРЎР‚РЎвЂ“Р В·Р Р†Р С‘РЎвЂ°Р Вµ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°, Р Р…Р Вµ Р С—Р С•РЎР‚Р С•Р В¶Р Р…РЎвЂќ.</param>
    /// <param name="readerEmail">Р СџР С•РЎв‚¬РЎвЂљР В° РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР В°, Р СРЎвЂ“РЎРѓРЎвЂљР С‘РЎвЂљРЎРЉ РЎРѓР С‘Р СР Р†Р С•Р В» @.</param>
    public static void EnsureHeaderValid(
        int loanId, string readerName, string readerEmail)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(loanId);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerEmail);
        if (!readerEmail.Contains('@'))
        {
            throw new ArgumentException(
                "Р СџР С•РЎв‚¬РЎвЂљР В° Р СР В°РЎвЂќ Р СРЎвЂ“РЎРѓРЎвЂљР С‘РЎвЂљР С‘ РЎРѓР С‘Р СР Р†Р С•Р В» @.",
                nameof(readerEmail));
        }
    }

    /// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚РЎРЏРЎвЂќ Р С•Р Т‘Р С‘Р Р… РЎР‚РЎРЏР Т‘Р С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    /// <param name="line">Р В РЎРЏР Т‘Р С•Р С” Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“: ISBN, Р С”РЎвЂ“Р В»РЎРЉР С”РЎвЂ“РЎРѓРЎвЂљРЎРЉ, РЎвЂ РЎвЂ“Р Р…Р В°.</param>
    public static void EnsureLineValid(LoanLine line)
    {
        ArgumentNullException.ThrowIfNull(line);
        ArgumentException.ThrowIfNullOrWhiteSpace(line.Isbn);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            line.Quantity, nameof(line.Quantity));
        ArgumentOutOfRangeException.ThrowIfNegative(
            line.UnitPrice, nameof(line.UnitPrice));
    }

    /// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚РЎРЏРЎвЂќ Р С—Р ВµРЎР‚Р ВµР В»РЎвЂ“Р С” РЎР‚РЎРЏР Т‘Р С”РЎвЂ“Р Р† Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    /// <param name="lines">Р В РЎРЏР Т‘Р С”Р С‘ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“, Р С—Р ВµРЎР‚Р ВµР В»РЎвЂ“Р С” Р Р…Р Вµ Р С—Р С•РЎР‚Р С•Р В¶Р Р…РЎвЂ“Р в„–.</param>
    public static void EnsureLinesValid(IReadOnlyList<LoanLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        if (lines.Count == 0)
        {
            throw new ArgumentException(
                "Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎР В° Р Р…Р Вµ Р СРЎвЂ“РЎРѓРЎвЂљР С‘РЎвЂљРЎРЉ Р В¶Р С•Р Т‘Р Р…Р С•РЎвЂ” Р С”Р Р…Р С‘Р В¶Р С”Р С‘.",
                nameof(lines));
        }

        foreach (LoanLine line in lines)
        {
            EnsureLineValid(line);
        }
    }

    /// <summary>Р СџР ВµРЎР‚Р ВµР Р†РЎвЂ“РЎР‚РЎРЏРЎвЂќ Р Р†Р ВµРЎРѓРЎРЉ Р В·Р В°Р С—Р С‘РЎвЂљ Р Р…Р В° РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С•Р С” Р С—РЎвЂ“Р Т‘РЎРѓРЎС“Р СР С”РЎС“.</summary>
    /// <param name="request">Р вЂ”Р В°Р С—Р С‘РЎвЂљ Р Р…Р В° РЎР‚Р С•Р В·РЎР‚Р В°РЎвЂ¦РЎС“Р Р…Р С•Р С”.</param>
    public static void EnsureRequestValid(LoanTotalRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Reader);
        EnsureHeaderValid(
            request.LoanId, request.Reader.Name, request.Reader.Email);
        EnsureLinesValid(request.Lines);
        ArgumentOutOfRangeException.ThrowIfNegative(request.DeliveryPrice);
    }
}
