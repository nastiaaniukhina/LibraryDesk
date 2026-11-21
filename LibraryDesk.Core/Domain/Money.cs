namespace LibraryDesk.Core.Domain;

/// <summary>
/// Забезпечує округлення грошових значень.
/// </summary>
public static class Money
{
    /// <summary>
    /// Кількість знаків після коми для грошових значень.
    /// </summary>
    public const int Digits = 2;

    /// <summary>
    /// Округлює грошове значення за правилом AwayFromZero.
    /// </summary>
    /// <param name="amount">Сума для округлення.</param>
    /// <returns>Округлена сума.</returns>
    public static decimal Round(decimal amount)
    {
        return Math.Round(amount, Digits, MidpointRounding.AwayFromZero);
    }
}
