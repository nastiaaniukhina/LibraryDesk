namespace LibraryDesk.Core.Reports;

/// <summary>Р’РёРІРѕРґРёС‚СЊ РіРѕС‚РѕРІРёР№ Р·РІС–С‚ Сѓ Р·Р°РґР°РЅРёР№ РїРѕС‚С–Рє.</summary>
public sealed class LoanReportPrinter
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoanReportPrinter"/> class.
    /// </summary>
    /// <param name="output">РџРѕС‚С–Рє, Сѓ СЏРєРёР№ РІРёРІРѕРґРёС‚СЊСЃСЏ Р·РІС–С‚.</param>
    public LoanReportPrinter(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <summary>Р—Р°РїРёСЃСѓС” Р·РІС–С‚ Сѓ РїРѕС‚С–Рє РІРёРІРѕРґСѓ.</summary>
    /// <param name="report">РўРµРєСЃС‚ Р·РІС–С‚Сѓ, РЅРµ РїРѕСЂРѕР¶РЅС–Р№.</param>
    public void Print(string report)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(report);
        _output.Write(report);
    }
}
