
using LibraryDesk.Core.Domain;
using Xunit;

namespace LibraryDesk.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesBook()
    {
        var book = new Book("ISBN1", "Назва", "Автор", 10m, 5);

        Assert.Equal("ISBN1", book.Isbn);
        Assert.Equal("Назва", book.Title);
        Assert.Equal("Автор", book.Author);
        Assert.Equal(10m, book.DailyRate);
        Assert.Equal(5, book.CopiesAvailable);
    }

    [Theory]
    [InlineData("", "Назва", "Автор")]
    [InlineData("ISBN1", "", "Автор")]
    [InlineData("ISBN1", "Назва", "")]
    public void Constructor_WithEmptyRequiredField_ThrowsArgumentException(
        string isbn, string title, string author)
    {
        Assert.Throws<ArgumentException>(
            () => new Book(isbn, title, author, 10m, 5));
    }

    [Fact]
    public void Constructor_WithNullIsbn_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Book(null!, "Назва", "Автор", 10m, 5));
    }

    [Fact]
    public void Constructor_WithNegativeDailyRate_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Book("ISBN1", "Назва", "Автор", -1m, 5));
    }

    [Fact]
    public void Constructor_WithNegativeCopies_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Book("ISBN1", "Назва", "Автор", 10m, -1));
    }

    [Fact]
    public void TakeCopy_DecreasesAvailableCopies()
    {
        var book = new Book("ISBN1", "Назва", "Автор", 10m, 5);

        book.TakeCopy();

        Assert.Equal(4, book.CopiesAvailable);
    }

    [Fact]
    public void TakeCopy_WhenNoCopiesAvailable_ThrowsInvalidOperationException()
    {
        var book = new Book("ISBN1", "Назва", "Автор", 10m, 0);

        Assert.Throws<InvalidOperationException>(() => book.TakeCopy());
        Assert.Equal(0, book.CopiesAvailable);
    }

    [Fact]
    public void ReturnCopy_IncreasesAvailableCopies()
    {
        var book = new Book("ISBN1", "Назва", "Автор", 10m, 2);

        book.ReturnCopy();

        Assert.Equal(3, book.CopiesAvailable);
    }

    [Fact]
    public void TakeCopyAndReturnCopy_RestoreOriginalCount()
    {
        var book = new Book("ISBN1", "Назва", "Автор", 10m, 2);

        book.TakeCopy();
        book.ReturnCopy();

        Assert.Equal(2, book.CopiesAvailable);
    }
}
