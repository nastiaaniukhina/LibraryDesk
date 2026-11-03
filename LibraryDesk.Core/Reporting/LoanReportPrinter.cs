namespace LibraryDesk.Core.Reporting;

/// <summary>Виводить готовий звіт у заданий потік.</summary>
public sealed class LoanReportPrinter
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoanReportPrinter"/> class.
    /// </summary>
    /// <param name="output">Потік, у який виводиться звіт.</param>
    public LoanReportPrinter(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <summary>Записує звіт у потік виводу.</summary>
    /// <param name="report">Текст звіту, не порожній.</param>
    public void Print(string report)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(report);
        _output.Write(report);
    }
}
