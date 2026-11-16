
using LibraryDesk.Core.Lending;
using Xunit;

namespace LibraryDesk.Tests;

public class BookLoanTests
{
    [Fact]
    public void FindById_WhenLoanExists_ReturnsLoan()
    {
        var loan = new BookLoan("L001", "Іваненко");
        var loans = new List<BookLoan> { loan };

        Assert.Same(loan, BookLoan.FindById(loans, "L001"));
    }

    [Fact]
    public void FindById_WhenLoanDoesNotExist_ReturnsNull()
    {
        var loans = new List<BookLoan>
        {
            new("L001", "Іваненко"),
        };

        Assert.Null(BookLoan.FindById(loans, "L002"));
    }

    [Fact]
    public void FindById_WhenListIsEmpty_ReturnsNull()
    {
        Assert.Null(BookLoan.FindById(new List<BookLoan>(), "L001"));
    }

    [Fact]
    public void CalculateTotal_WithNoLines_ReturnsZero()
    {
        var loan = new BookLoan("L001", "Іваненко");

        Assert.Equal(0m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_AddsVatToLineTotal()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 2, 100m);

        Assert.Equal(240m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_RegularReaderGetsDiscountAboveThreshold()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 1001m);

        Assert.Equal(1081.08m, loan.CalculateTotal(true));
    }

    [Fact]
    public void CalculateTotal_RegularReaderDoesNotGetDiscountAtThreshold()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 1000m);

        Assert.Equal(1200m, loan.CalculateTotal(true));
    }

    [Fact]
    public void CalculateTotal_LargeOrderGetsDiscountAboveThreshold()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 5001m);

        Assert.Equal(5101.02m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_LargeOrderDoesNotGetDiscountAtThreshold()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 5000m);

        Assert.Equal(6000m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_RegularDiscountHasPriorityOverLargeOrderDiscount()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 6000m);

        Assert.Equal(6480m, loan.CalculateTotal(true));
    }

    [Fact]
    public void CalculateTotal_BulkDiscountAppliesToMoreThanTenLines()
    {
        var loan = new BookLoan("L001", "Іваненко");

        for (int i = 0; i < 11; i++)
        {
            loan.AddLine($"ISBN{i}", 1, 20m);
        }

        Assert.Equal(144m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_BulkDiscountDoesNotApplyToTenLines()
    {
        var loan = new BookLoan("L001", "Іваненко");

        for (int i = 0; i < 10; i++)
        {
            loan.AddLine($"ISBN{i}", 1, 20m);
        }

        Assert.Equal(240m, loan.CalculateTotal(false));
    }

    [Fact]
    public void CalculateTotal_DoesNotReturnNegativeAmount()
    {
        var loan = new BookLoan("L001", "Іваненко");

        for (int i = 0; i < 11; i++)
        {
            loan.AddLine($"ISBN{i}", 1, 1m);
        }

        Assert.Equal(0m, loan.CalculateTotal(false));
    }

    [Fact]
    public void TryChangeStatus_ReservedToIssued_ReturnsTrue()
    {
        var loan = new BookLoan("L001", "Іваненко");

        Assert.True(loan.TryChangeStatus(BookLoan.StatusIssued));
        Assert.Equal(BookLoan.StatusIssued, loan.Status);
    }

    [Fact]
    public void TryChangeStatus_IssuedToReturned_ReturnsTrue()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.TryChangeStatus(BookLoan.StatusIssued);

        Assert.True(loan.TryChangeStatus(BookLoan.StatusReturned));
        Assert.Equal(BookLoan.StatusReturned, loan.Status);
    }

    [Fact]
    public void TryChangeStatus_ReservedToCancelled_ReturnsTrue()
    {
        var loan = new BookLoan("L001", "Іваненко");

        Assert.True(loan.TryChangeStatus(BookLoan.StatusCancelled));
        Assert.Equal(BookLoan.StatusCancelled, loan.Status);
    }

    [Theory]
[InlineData(BookLoan.StatusReturned)]
[InlineData(99)]
public void TryChangeStatus_InvalidTransition_ReturnsFalse(int newStatus)
{
    var loan = new BookLoan("L001", "Іваненко");

    Assert.False(loan.TryChangeStatus(newStatus));
    Assert.Equal(BookLoan.StatusReserved, loan.Status);
}

    [Fact]
    public void TryChangeStatus_CannotChangeStatusAfterReturning()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.TryChangeStatus(BookLoan.StatusIssued);
        loan.TryChangeStatus(BookLoan.StatusReturned);

        Assert.False(loan.TryChangeStatus(BookLoan.StatusCancelled));
        Assert.Equal(BookLoan.StatusReturned, loan.Status);
    }

    [Fact]
    public void IsValid_WithCorrectData_ReturnsTrue()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 1, 100m);

        Assert.True(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithoutLines_ReturnsFalse()
    {
        var loan = new BookLoan("L001", "Іваненко");

        Assert.False(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithEmptyId_ReturnsFalse()
    {
        var loan = new BookLoan("", "Іваненко");
        loan.AddLine("ISBN1", 1, 100m);

        Assert.False(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithShortReaderName_ReturnsFalse()
    {
        var loan = new BookLoan("L001", "Ів");
        loan.AddLine("ISBN1", 1, 100m);

        Assert.False(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithNullReaderName_ReturnsFalse()
    {
        var loan = new BookLoan("L001", null!);
        loan.AddLine("ISBN1", 1, 100m);

        Assert.False(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithNinetyNineLines_ReturnsTrue()
    {
        var loan = new BookLoan("L001", "Іваненко");

        for (int i = 0; i < 99; i++)
        {
            loan.AddLine($"ISBN{i}", 1, 10m);
        }

        Assert.True(loan.IsValid());
    }

    [Fact]
    public void IsValid_WithOneHundredLines_ReturnsFalse()
    {
        var loan = new BookLoan("L001", "Іваненко");

        for (int i = 0; i < 100; i++)
        {
            loan.AddLine($"ISBN{i}", 1, 10m);
        }

        Assert.False(loan.IsValid());
    }

    [Fact]
    public void BuildReport_WithNoLines_ContainsTotal()
    {
        var loan = new BookLoan("L001", "Іваненко");

        Assert.Contains("Разом: 0", loan.BuildReport());
    }

    [Fact]
    public void BuildReport_WithLine_ContainsBookDetailsAndTotal()
    {
        var loan = new BookLoan("L001", "Іваненко");
        loan.AddLine("ISBN1", 2, 100m);

        string report = loan.BuildReport();

        Assert.Contains("Книжка: ISBN1", report);
        Assert.Contains("кількість: 2", report);
        Assert.Contains("ціна: 100", report);
        Assert.Contains("сума: 200", report);
        Assert.Contains("Разом: 240", report);
    }
}
