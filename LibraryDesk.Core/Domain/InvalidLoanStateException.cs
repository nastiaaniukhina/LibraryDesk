namespace LibraryDesk.Core.Domain;

/// <summary>
/// Represents an error caused by an invalid loan state.
/// </summary>
public class InvalidLoanStateException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLoanStateException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidLoanStateException(string message)
        : base(message)
    {
    }
}
