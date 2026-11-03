using System.Text;
using LibraryDesk.Core.Lending;
using LibraryDesk.Core.Reporting;

Console.OutputEncoding = new UTF8Encoding(false);
Console.InputEncoding = new UTF8Encoding(false);

BookLoan loan = new("L-1001", "Іваненко");
loan.AddLine("ISBN-001", 3, 250m);
loan.AddLine("ISBN-002", 12, 90m);
Console.WriteLine(loan.CalculateTotal(false));
Console.WriteLine(loan.CalculateTotal(true));
Console.WriteLine(loan.IsValid());
Console.WriteLine(loan.TryChangeStatus(1));
Console.Write(loan.BuildReport());
Console.WriteLine();
LoanTotalRequest request = new(
    LoanId: 1001,
    Reader: new ReaderInfo("Іваненко", "ivanenko@ex.com", true),
    Lines: new List<LoanLine>
    {
        new() { Isbn = "ISBN-001", Quantity = 12, UnitPrice = 100m },
        new() { Isbn = "ISBN-002", Quantity = 1, UnitPrice = 250m },
    },
    CreatedAt: new DateOnly(2026, 3, 10),
    CouponCode: null,
    Status: LoanFormStatus.Reserved,
    DeliveryPrice: 60m);
LoanTotalResult result = LoanTotalCalculator.Calculate(request);
string report = LoanReport.TextOf(request, result);
LoanReportPrinter printer = new(Console.Out);
printer.Print(report);
