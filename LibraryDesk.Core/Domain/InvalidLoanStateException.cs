namespace LibraryDesk.Core.Domain;

public class InvalidLoanStateException : DomainRuleException
{
    public InvalidLoanStateException(string message) : base(message)
    {
    }
}
