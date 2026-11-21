namespace LibraryDesk.Core.Domain;

public class LoanNotFoundException : DomainRuleException
{
    public LoanNotFoundException(string message) : base(message)
    {
    }
}
