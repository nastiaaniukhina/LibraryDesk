using LibraryDesk.Core.Reports;
using LibraryDesk.Labs;
using LibraryDesk.Labs.Lending;

namespace LibraryDesk.Tests;

public class LoanReportTests
{
    [Fact]
    public void TextOf_ContainsTotalLine()
    {
        LoanTotalRequest request = BuildSampleRequest();
        LoanTotalResult result = LoanTotalCalculator.Calculate(request);

        string text = LoanReport.TextOf(request, result);
        Assert.Contains("Р Р°Р·РѕРј: 1292.70", text);
    }

    [Fact]
    public void TextOf_NullRequest_ThrowsArgumentNull()
    {
        LoanTotalResult result = new(0m, LoanFormStatus.Reserved);

        Assert.Throws<ArgumentNullException>(
            () => LoanReport.TextOf(null!, result));
    }

    [Fact]
    public void Print_WritesReportToOutput()
    {
        StringWriter writer = new();
        LoanReportPrinter printer = new(writer);

        printer.Print("Р В Р В°Р В·Р С•Р С: 10.00");

        Assert.Contains("10.00", writer.ToString());
    }

    private static LoanTotalRequest BuildSampleRequest()
    {
        return new LoanTotalRequest(
            LoanId: 1,
            Reader: new ReaderInfo("Р†РІР°РЅРµРЅРєРѕ", "i@ex.com", true),
            Lines: new List<LoanLine>
            {
                new() { Isbn = "ISBN-001", Quantity = 12, UnitPrice = 100m },
                new() { Isbn = "ISBN-002", Quantity = 1, UnitPrice = 250m },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: null,
            Status: LoanFormStatus.Reserved,
            DeliveryPrice: 60m);
    }
}
