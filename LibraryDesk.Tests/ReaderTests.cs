
using LibraryDesk.Core.Domain;
using Xunit;

namespace LibraryDesk.Tests;

public class ReaderTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesReader()
    {
        var reader = new Reader(1, "Іваненко", "test@example.com");

        Assert.Equal(1, reader.Id);
        Assert.Equal("Іваненко", reader.Name);
        Assert.Equal("test@example.com", reader.Email);
        Assert.False(reader.IsStudent);
    }

    [Fact]
    public void Constructor_WithZeroId_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Reader(0, "Іваненко", "test@example.com"));
    }

    [Fact]
    public void Constructor_WithNegativeId_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Reader(-1, "Іваненко", "test@example.com"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithEmptyName_ThrowsArgumentException(string name)
    {
        Assert.Throws<ArgumentException>(
            () => new Reader(1, name, "test@example.com"));
    }

    [Fact]
    public void Constructor_WithNullName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Reader(1, null!, "test@example.com"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithEmptyEmail_ThrowsArgumentException(string email)
    {
        Assert.Throws<ArgumentException>(
            () => new Reader(1, "Іваненко", email));
    }

    [Fact]
    public void Constructor_WithNullEmail_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Reader(1, "Іваненко", null!));
    }

    [Fact]
    public void Constructor_WithEmailWithoutAtSign_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => new Reader(1, "Іваненко", "test.example.com"));
    }

    [Fact]
    public void IsStudent_CanBeSetDuringInitialization()
    {
        var reader = new Reader(1, "Іваненко", "test@example.com")
        {
            IsStudent = true,
        };

        Assert.True(reader.IsStudent);
    }
}
