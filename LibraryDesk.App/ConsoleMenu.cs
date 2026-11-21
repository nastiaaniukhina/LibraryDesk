using System.Globalization;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Reports;
using LibraryDesk.Core.Services;

namespace LibraryDesk.App;

/// <summary>РљРѕРЅСЃРѕР»СЊРЅРµ РјРµРЅСЋ СЃС†РµРЅР°СЂС–С—РІ Р±С–Р±Р»С–РѕС‚РµРєРё.</summary>
public sealed class ConsoleMenu
{
    private readonly LoanLifecycleService _lifecycle;
    private readonly LoanQueryService _queries;
    private readonly ScenarioRunner _runner;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    /// <summary>РЎС‚РІРѕСЂСЋС” РјРµРЅСЋ Р· РїРѕС‚РѕРєР°РјРё РІРІРµРґРµРЅРЅСЏ С‚Р° РІРёРІРµРґРµРЅРЅСЏ.</summary>
    public ConsoleMenu(LoanLifecycleService lifecycle, LoanQueryService queries, ScenarioRunner runner, TextReader input, TextWriter output)
    {
        _lifecycle = lifecycle;
        _queries = queries;
        _runner = runner;
        _input = input;
        _output = output;
    }

    /// <summary>Р—Р°РїСѓСЃРєР°С” РіРѕР»РѕРІРЅРёР№ С†РёРєР» РјРµРЅСЋ.</summary>
    public int Run()
    {
        while (true)
        {
            ShowMenu();
            string? choice = _input.ReadLine();
            if (choice == "0")
            {
                return 0;
            }

            Execute(choice);
        }
    }

    private void ShowMenu()
    {
        _output.WriteLine("1. Р’РёРґР°С‚Рё РєРЅРёР¶РєРё");
        _output.WriteLine("2. РџСЂРёР№РЅСЏС‚Рё РїРѕРІРµСЂРЅРµРЅРЅСЏ");
        _output.WriteLine("3. РЎРєР°СЃСѓРІР°С‚Рё Р±СЂРѕРЅСЊ");
        _output.WriteLine("4. Р—РЅР°Р№С‚Рё Р·Р° С‡РёС‚Р°С‡РµРј");
        _output.WriteLine("5. Р—РЅР°Р№С‚Рё Р·Р° ISBN");
        _output.WriteLine("6. Р—РІС–С‚ Р±РѕСЂР¶РЅРёРєС–РІ");
        _output.WriteLine("0. Р’РёС…С–Рґ");
    }

    private void Execute(string? choice)
    {
        switch (choice)
        {
            case "1": RunLoanAction("UC-02", _lifecycle.Issue); break;
            case "2": ReturnLoan(); break;
            case "3": RunLoanAction("UC-04", _lifecycle.Cancel); break;
            case "4": FindReader(); break;
            case "5": FindIsbn(); break;
            case "6": DebtorReport(); break;
            default: _output.WriteLine("РќРµРІС–РґРѕРјРёР№ РїСѓРЅРєС‚ РјРµРЅСЋ."); break;
        }
    }

    private void RunLoanAction(string scenario, Action<int> action)
    {
        if (!ReadId(out int id))
        {
            return;
        }

        _runner.Run(scenario, () => action(id));
    }

    private void ReturnLoan()
    {
        if (!ReadId(out int id))
        {
            return;
        }

        _runner.Run("UC-03", () =>
        {
            ReturnReceipt receipt = _lifecycle.ReturnLoan(id);
            _output.WriteLine($"РЎСѓРјР°: {receipt.Total.ToString("F2", CultureInfo.InvariantCulture)}");
            if (receipt.OverdueDays > 0)
            {
                _output.WriteLine($"РџРµРЅСЏ: {receipt.Penalty.ToString("F2", CultureInfo.InvariantCulture)}");
            }
        });
    }

    private bool ReadId(out int id)
    {
        _output.Write("РќРѕРјРµСЂ РІРёРґР°С‡С–: ");
        if (InputParsers.TryLoanId(_input.ReadLine(), out id))
        {
            return true;
        }

        _output.WriteLine("РќРµРєРѕСЂРµРєС‚РЅРёР№ РЅРѕРјРµСЂ.");
        return false;
    }

    private void FindReader()
    {
        _output.Write("ID С‡РёС‚Р°С‡Р°: ");
        if (!int.TryParse(_input.ReadLine(), out int id) || id <= 0) { _output.WriteLine("РќРµРєРѕСЂРµРєС‚РЅРёР№ ID."); return; }
        foreach (Loan loan in _queries.FindByReader(id))
        {
            _output.WriteLine($"#{loan.Id}: {loan.Status}");
        }
    }

    private void FindIsbn()
    {
        _output.Write("ISBN: ");
        string? isbn = _input.ReadLine();
        if (string.IsNullOrWhiteSpace(isbn)) { _output.WriteLine("ISBN РЅРµ РјРѕР¶Рµ Р±СѓС‚Рё РїРѕСЂРѕР¶РЅС–Рј."); return; }
        foreach (Loan loan in _queries.FindByIsbn(isbn))
        {
            _output.WriteLine($"#{loan.Id}: {loan.Status}");
        }
    }

    private void DebtorReport()
    {
        _output.Write("Р”Р°С‚Р° yyyy-MM-dd: ");
        if (!InputParsers.TryDate(_input.ReadLine(), out DateOnly date)) { _output.WriteLine("РќРµРєРѕСЂРµРєС‚РЅР° РґР°С‚Р°."); return; }
        IReadOnlyList<DebtorLine> lines = _queries.OverdueReport(date);
        _output.WriteLine(OverdueReportFormatter.Format(lines));
    }
}
