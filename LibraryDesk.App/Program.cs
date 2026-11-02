using System.Text;
using LibraryDesk.Core.Lending;

Console.OutputEncoding = new UTF8Encoding(false);
Console.InputEncoding = new UTF8Encoding(false);

zakaz z = new("L-1001", "Іваненко");
z.Add("ISBN-001", 3, 250m);
z.Add("ISBN-002", 12, 90m);
Console.WriteLine(z.ProcessData(false));
Console.WriteLine(z.ProcessData(true));
Console.WriteLine(z.Ck());
Console.WriteLine(z.Chg(1));
Console.Write(z.Rep());