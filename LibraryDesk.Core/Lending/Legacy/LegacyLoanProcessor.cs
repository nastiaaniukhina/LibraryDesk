#pragma warning disable

namespace LibraryDesk.Core.Lending.Legacy;

public static class LegacyLoanProcessor
{
    public static decimal Process(
        int docId, string clientName, string clientEmail,
        bool isRegular, List<LoanLine> lines,
        DateOnly createdAt, string couponCode, string status,
        bool printToConsole, decimal deliveryPrice,
        out string newStatus)
    {
        newStatus = status;
        decimal total = 0;
        if (docId <= 0)
        {
            Console.WriteLine("Помилка: номер видачі");
            return -1;
        }
        if (clientName == null || clientName == "")
        {
            Console.WriteLine("Помилка: немає читача");
            return -1;
        }
        if (clientEmail == null || !clientEmail.Contains("@"))
        {
            Console.WriteLine("Помилка: пошта читача");
            return -1;
        }
        if (lines == null || lines.Count == 0)
        {
            Console.WriteLine("Помилка: немає книжок");
            return -1;
        }
        if (status != "Reserved" && status != "Issued" &&
            status != "Returned" && status != "Cancelled")
        {
            Console.WriteLine("Помилка: невідомий стан");
            return -1;
        }
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Quantity <= 0)
            {
                Console.WriteLine("Помилка: кількість");
                return -1;
            }
            if (lines[i].UnitPrice < 0)
            {
                Console.WriteLine("Помилка: ціна");
                return -1;
            }
            decimal sum = lines[i].Quantity * lines[i].UnitPrice;
            if (lines[i].Quantity >= 10)
                sum = sum * 0.95m;
            total = total + sum;
        }
        if (isRegular)
            total = total * 0.93m;
        if (total > 5000)
            total = total * 0.9m;
        else if (total > 2000)
            total = total * 0.95m;
        if (couponCode == "SALE10")
            total = total - total * 0.1m;
        else if (couponCode == "MINUS200" && total > 1000)
            total = total - 200;
        if (createdAt.DayOfWeek == DayOfWeek.Sunday)
            total = total * 0.98m;
        if (total < 1000)
            total = total + deliveryPrice;
        if (total < 0)
            total = 0;
        total = Math.Round(total, 2);
        if (status == "Reserved" && total > 0)
            newStatus = "Issued";
        else if (status == "Issued")
            newStatus = "Returned";
        else if (status == "Cancelled")
        {
            newStatus = "Cancelled";
            total = 0;
        }
        if (printToConsole)
        {
            Console.WriteLine("Видача № " + docId);
            Console.WriteLine("Читач: " + clientName);
            Console.WriteLine("Пошта: " + clientEmail);
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i].Isbn + " x " +
                    lines[i].Quantity);
            }
            Console.WriteLine("Разом: " + total);
            Console.WriteLine("Стан: " + newStatus);
        }
        return total;
    }
}