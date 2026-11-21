using System.Globalization;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Reports;
using LibraryDesk.Core.Services;

namespace LibraryDesk.App;

/// <summary>Консольне меню сценаріїв бібліотеки.</summary>
public sealed class ConsoleMenu
{
    private readonly LoanLifecycleService _lifecycle;
    private readonly LoanQueryService _queries;
    private readonly ScenarioRunner _runner;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    /// <summary>Створює меню з потоками введення та виведення.</summary>
    public ConsoleMenu(LoanLifecycleService lifecycle, LoanQueryService queries, ScenarioRunner runner, TextReader input, TextWriter output)
    {
        _lifecycle = lifecycle;
        _queries = queries;
        _runner = runner;
        _input = input;
        _output = output;
    }

    /// <summary>Запускає головний цикл меню.</summary>
    public int Run()
    {
        while (true)
        {
            ShowMenu();
            string? choice = _input.ReadLine();
            if (choice == "0") return 0;
            Execute(choice);
        }
    }

    private void ShowMenu()
    {
        _output.WriteLine("1. Видати книжки");
        _output.WriteLine("2. Прийняти повернення");
        _output.WriteLine("3. Скасувати бронь");
        _output.WriteLine("4. Знайти за читачем");
        _output.WriteLine("5. Знайти за ISBN");
        _output.WriteLine("6. Звіт боржників");
        _output.WriteLine("0. Вихід");
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
            default: _output.WriteLine("Невідомий пункт меню."); break;
        }
    }

    private void RunLoanAction(string scenario, Action<int> action)
    {
        if (!ReadId(out int id)) return;
        _runner.Run(scenario, () => action(id));
    }

    private void ReturnLoan()
    {
        if (!ReadId(out int id)) return;
        _runner.Run("UC-03", () =>
        {
            ReturnReceipt receipt = _lifecycle.ReturnLoan(id);
            _output.WriteLine($"Сума: {receipt.Total.ToString("F2", CultureInfo.InvariantCulture)}");
            if (receipt.OverdueDays > 0) _output.WriteLine($"Пеня: {receipt.Penalty.ToString("F2", CultureInfo.InvariantCulture)}");
        });
    }

    private bool ReadId(out int id)
    {
        _output.Write("Номер видачі: ");
        if (InputParsers.TryLoanId(_input.ReadLine(), out id)) return true;
        _output.WriteLine("Некоректний номер.");
        return false;
    }

    private void FindReader()
    {
        _output.Write("ID читача: ");
        if (!int.TryParse(_input.ReadLine(), out int id) || id <= 0) { _output.WriteLine("Некоректний ID."); return; }
        foreach (Loan loan in _queries.FindByReader(id)) _output.WriteLine($"#{loan.Id}: {loan.Status}");
    }

    private void FindIsbn()
    {
        _output.Write("ISBN: ");
        string? isbn = _input.ReadLine();
        if (string.IsNullOrWhiteSpace(isbn)) { _output.WriteLine("ISBN не може бути порожнім."); return; }
        foreach (Loan loan in _queries.FindByIsbn(isbn)) _output.WriteLine($"#{loan.Id}: {loan.Status}");
    }

    private void DebtorReport()
    {
        _output.Write("Дата yyyy-MM-dd: ");
        if (!InputParsers.TryDate(_input.ReadLine(), out DateOnly date)) { _output.WriteLine("Некоректна дата."); return; }
        IReadOnlyList<DebtorLine> lines = _queries.OverdueReport(date);
        _output.WriteLine(OverdueReportFormatter.Format(lines));
    }
}