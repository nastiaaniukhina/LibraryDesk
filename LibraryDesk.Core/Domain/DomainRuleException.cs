namespace LibraryDesk.Core.Domain;

/// <summary>
/// Represents an error that occurs when a domain rule is violated.
/// </summary>
public class DomainRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public DomainRuleException(string message)
        : base(message)
    {
    }
}
