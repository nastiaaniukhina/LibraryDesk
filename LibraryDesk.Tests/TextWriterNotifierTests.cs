
using LibraryDesk.Core.Notifications;
using Xunit;

namespace LibraryDesk.Tests;

public class TextWriterNotifierTests
{
    [Fact]
    public void Constructor_WithNullOutput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new TextWriterNotifier(null!));
    }

    [Fact]
    public void Notify_WritesMessageToOutput()
    {
        using var writer = new StringWriter();
        var notifier = new TextWriterNotifier(writer);

        notifier.Notify("test@example.com", "Видачу створено.");

        Assert.Equal("Лист для test@example.com: Видачу створено." + Environment.NewLine,
            writer.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Notify_WithEmptyRecipient_ThrowsArgumentException(string recipient)
    {
        using var writer = new StringWriter();
        var notifier = new TextWriterNotifier(writer);

        Assert.Throws<ArgumentException>(
            () => notifier.Notify(recipient, "Повідомлення"));
    }

    [Fact]
    public void Notify_WithNullRecipient_ThrowsArgumentNullException()
    {
        using var writer = new StringWriter();
        var notifier = new TextWriterNotifier(writer);

        Assert.Throws<ArgumentNullException>(
            () => notifier.Notify(null!, "Повідомлення"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Notify_WithEmptyMessage_ThrowsArgumentException(string message)
    {
        using var writer = new StringWriter();
        var notifier = new TextWriterNotifier(writer);

        Assert.Throws<ArgumentException>(
            () => notifier.Notify("test@example.com", message));
    }

    [Fact]
    public void Notify_WithNullMessage_ThrowsArgumentNullException()
    {
        using var writer = new StringWriter();
        var notifier = new TextWriterNotifier(writer);

        Assert.Throws<ArgumentNullException>(
            () => notifier.Notify("test@example.com", null!));
    }
}
