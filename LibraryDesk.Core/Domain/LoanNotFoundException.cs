namespace LibraryDesk.Core.Domain;

/// <summary>
/// Represents an error that occurs when a loan cannot be found.
/// </summary>
public class LoanNotFoundException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoanNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public LoanNotFoundException(string message)
        : base(message)
    {
    }
}
