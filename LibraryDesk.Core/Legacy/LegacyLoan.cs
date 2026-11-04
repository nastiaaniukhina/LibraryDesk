#pragma warning disable
namespace LibraryDesk.Legacy;

public class LegacyLoanItem
{
    public string Isbn { get; set; } = "";
    public int Days { get; set; }
    public decimal DailyRate { get; set; }
}

public class LegacyLoan
{
    public int Id { get; set; }
    public int Status { get; set; }
    public decimal Total { get; set; }
    public string Email { get; set; } = "";
    public List<LegacyLoanItem> Items { get; set; } = new();

    public virtual void AddItem(LegacyLoanItem item) => Items.Add(item);
}

public class LegacyArchivedLoan : LegacyLoan
{
    public LegacyArchivedLoan(int id) => Id = id;

    public override void AddItem(LegacyLoanItem item)
    {
        throw new NotSupportedException("Архів змінювати не можна");
    }
}
