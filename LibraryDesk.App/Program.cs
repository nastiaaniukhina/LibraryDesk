using System.Text;
using LibraryDesk.Core.Lending;

Console.OutputEncoding = new UTF8Encoding(false);
Console.InputEncoding = new UTF8Encoding(false);

BookLoan loan = new("L-1001", "Іваненко");
loan.AddLine("ISBN-001", 3, 250m);
loan.AddLine("ISBN-002", 12, 90m);
Console.WriteLine(loan.CalculateTotal(false));
Console.WriteLine(loan.CalculateTotal(true));
Console.WriteLine(loan.IsValid());
Console.WriteLine(loan.TryChangeStatus(1));
Console.Write(loan.BuildReport());
