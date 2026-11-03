namespace LibraryDesk.Core.Lending;

/// <summary>Перевірки передумов для даних видачі.</summary>
public static class LoanGuards
{
    /// <summary>Перевіряє заголовок видачі: номер, прізвище читача, пошту.</summary>
    /// <param name="loanId">Номер видачі, має бути додатним.</param>
    /// <param name="readerName">Прізвище читача, не порожнє.</param>
    /// <param name="readerEmail">Пошта читача, містить символ @.</param>
    public static void EnsureHeaderValid(
        int loanId, string readerName, string readerEmail)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(loanId);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(readerEmail);
        if (!readerEmail.Contains('@'))
        {
            throw new ArgumentException(
                "Пошта має містити символ @.",
                nameof(readerEmail));
        }
    }

    /// <summary>Перевіряє один рядок видачі.</summary>
    /// <param name="line">Рядок видачі: ISBN, кількість, ціна.</param>
    public static void EnsureLineValid(LoanLine line)
    {
        ArgumentNullException.ThrowIfNull(line);
        ArgumentException.ThrowIfNullOrWhiteSpace(line.Isbn);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            line.Quantity, nameof(line.Quantity));
        ArgumentOutOfRangeException.ThrowIfNegative(
            line.UnitPrice, nameof(line.UnitPrice));
    }

    /// <summary>Перевіряє перелік рядків видачі.</summary>
    /// <param name="lines">Рядки видачі, перелік не порожній.</param>
    public static void EnsureLinesValid(IReadOnlyList<LoanLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        if (lines.Count == 0)
        {
            throw new ArgumentException(
                "Видача не містить жодної книжки.",
                nameof(lines));
        }

        foreach (LoanLine line in lines)
        {
            EnsureLineValid(line);
        }
    }
}
