using LibraryDesk.Core.Lending;
using Xunit;

namespace LibraryDesk.Tests;

public class LoanGuardsTests
{
    [Fact]
    public void EnsureLinesValid_NoLines_ThrowsArgument()
    {
        Assert.Throws<ArgumentException>(
            () => LoanGuards.EnsureLinesValid(new List<LoanLine>()));
    }

    [Fact]
    public void EnsureLineValid_ZeroQuantity_ThrowsRange()
    {
        LoanLine line = new() { Isbn = "ISBN-001", Quantity = 0, UnitPrice = 10m };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => LoanGuards.EnsureLineValid(line));
    }

    [Fact]
    public void EnsureLinesValid_NullLines_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => LoanGuards.EnsureLinesValid(null!));
    }

    [Fact]
    public void EnsureHeaderValid_EmailWithoutAt_ThrowsArgument()
    {
        Assert.Throws<ArgumentException>(
            () => LoanGuards.EnsureHeaderValid(1, "Іваненко", "ivanenko.ex.com"));
    }
}
