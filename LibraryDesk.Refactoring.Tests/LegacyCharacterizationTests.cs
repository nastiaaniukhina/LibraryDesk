
using System.Globalization;
using LibraryDesk.Refactoring;
using Xunit;

public class LegacyCharacterizationTests
{
    public LegacyCharacterizationTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    private static List<LoanPosition> TwoLines() => new()
    {
        new LoanPosition { Code = "A-1", Qty = 2, Price = 150m },
        new LoanPosition { Code = "B-2", Qty = 1, Price = 400m }
    };

    [Theory]
    [InlineData("null", "new", "a@b.c", "ERR: null")]
    [InlineData("empty", "new", "a@b.c", "ERR: empty")]
    [InlineData("two", "draft", "a@b.c", "ERR: state")]
    [InlineData("two", "new", "no-mail", "ERR: mail")]
    public void Handle_BadInput_ReturnsErrorCode(
        string kindOfItems,
        string state,
        string mail,
        string expected)
    {
        var items = kindOfItems switch
        {
            "two" => TwoLines(),
            "empty" => new List<LoanPosition>(),
            _ => null
        };

        var sut = new LegacyLoanProcessor();

        var actual = sut.Handle(
            1001, 7, "Іван", mail, "regular", 0,
            items, state, "UAH",
            new DateTime(2026, 3, 1), false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Handle_VipOrder_ReturnsExactReport()
    {
        var sut = new LegacyLoanProcessor();

        var report = sut.Handle(
            1001, 7, "Іван", "a@b.c", "vip", 3,
            TwoLines(), "new", "UAH",
            new DateTime(2026, 3, 1), false);

        var expected =
            "Документ #1001\n" +
            "Клієнт: Іван\n" +
            "A-1 x2 = 300.00 UAH\n" +
            "B-2 x1 = 400.00 UAH\n" +
            "Знижка: 105.00 UAH\n" +
            "Доставка: 60.00 UAH\n" +
            "Разом: 655.00 UAH\n";

        Assert.Equal(expected, report);
    }

    [Theory]
    [InlineData("regular", 0, 760)]
    [InlineData("vip", 0, 655)]
    [InlineData("staff", 0, 550)]
    [InlineData("regular", 11, 725)]
    public void Preview_ByKind_ReturnsTotal(
        string kind,
        int done,
        decimal expected)
    {
        var sut = new LegacyLoanProcessor();

        var total = sut.Preview(kind, done, TwoLines());

        Assert.Equal(expected, total);
    }

    [Fact]
    public void Preview_AtFreeShippingLimit_Returns1000()
    {
        var sut = new LegacyLoanProcessor();

        var items = new List<LoanPosition>
        {
            new LoanPosition { Code = "C-1", Qty = 1, Price = 1000m }
        };

        var total = sut.Preview("regular", 0, items);

        Assert.Equal(1000m, total);
    }

    [Fact]
    public void DescribeClient_Vip_BuildsCaption()
    {
        var client = new Borrower
        {
            Name = " іван ",
            Email = "IVAN@MAIL.COM",
            Kind = "vip",
            DoneCount = 12,
            SinceUtc = new DateTime(2020, 1, 1)
        };

        var years = DateTime.Now.Year - 2020;
        var expected = "ІВАН [VIP] [ЛОЯЛЬНИЙ] " +
                       "<ivan@mail.com> стаж " + years;

        var actual = new LegacyLoanProcessor().DescribeClient(client);

        Assert.Equal(expected, actual);
    }
}
