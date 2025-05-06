using System;
using System.Collections.Generic;
using System.Linq;

class Artikul
{
    public int Number { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int Type { get; set; }

    public string GetUniqueCode()
    {
        string prefix = Name.Length >= 2 ? Name.Substring(0, 2) : Name.PadRight(2, '_');
        return $"{Number}{prefix}{Type}";
    }

    public double TotalValue()
    {
        return Price * Quantity;
    }

    public override string ToString()
    {
        return $"{Number}, {Name}, {Price:F2} лв., {Quantity} броя, {Type} тип, {GetUniqueCode()}";
    }
}

class Program
{
    static List<Artikul> artikuli = new List<Artikul>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nМЕНЮ:");
            Console.WriteLine("1. Въведи нов артикул");
            Console.WriteLine("2. Пълен списък на артикулите (сортирани по име)");
            Console.WriteLine("3. Артикули от даден тип (сортирани по обща стойност)");
            Console.WriteLine("4. Справка за артикули тип 4 (средна цена и обща стойност)");
            Console.WriteLine("5. Изход");
            Console.Write("Избор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddArtikul(); break;
                case "2": ListAllArtikuli(); break;
                case "3": ListArtikuliByType(); break;
                case "4": ReportType4(); break;
                case "5": return;
                default: Console.WriteLine("Невалиден избор!"); break;
            }
        }
    }

    static void AddArtikul()
    {
        if (artikuli.Count >= 2000)
        {
            Console.WriteLine("Достигнат е лимитът от 2000 артикула!");
            return;
        }

        var a = new Artikul();
        a.Number = artikuli.Count + 1;

        Console.Write("Име на артикул (2-20 символа): ");
        a.Name = Console.ReadLine();
        while (a.Name.Length < 2 || a.Name.Length > 20)
        {
            Console.Write("Грешка! Въведи име с 2 до 20 символа: ");
            a.Name = Console.ReadLine();
        }

        Console.Write("Единична цена (минимум 0.10): ");
        a.Price = double.Parse(Console.ReadLine());
        while (a.Price < 0.10)
        {
            Console.Write("Грешка! Въведи цена >= 0.10: ");
            a.Price = double.Parse(Console.ReadLine());
        }

        Console.Write("Наличност (цяло положително число): ");
        a.Quantity = int.Parse(Console.ReadLine());
        while (a.Quantity <= 0)
        {
            Console.Write("Грешка! Въведи положително число: ");
            a.Quantity = int.Parse(Console.ReadLine());
        }

        Console.Write("Тип (1-5): ");
        a.Type = int.Parse(Console.ReadLine());
        while (a.Type < 1 || a.Type > 5)
        {
            Console.Write("Грешка! Въведи тип между 1 и 5: ");
            a.Type = int.Parse(Console.ReadLine());
        }

        artikuli.Add(a);
        Console.WriteLine("Артикулът е добавен успешно!");
    }

    static void ListAllArtikuli()
    {
        if (artikuli.Count == 0)
        {
            Console.WriteLine("Няма въведени артикули.");
            return;
        }

        var sorted = artikuli.OrderBy(a => a.Name).ToList();
        Console.WriteLine("\nПълен списък на артикулите:");
        foreach (var a in sorted)
        {
            Console.WriteLine(a);
        }
    }

    static void ListArtikuliByType()
    {
        Console.Write("Въведи тип (1-5) за търсене: ");
        int searchType = int.Parse(Console.ReadLine());

        var filtered = artikuli
            .Where(a => a.Type == searchType)
            .OrderBy(a => a.TotalValue())
            .ToList();

        if (filtered.Count == 0)
        {
            Console.WriteLine($"Няма артикули от тип {searchType}.");
            return;
        }

        Console.WriteLine($"\nАртикули от тип {searchType} (сортирани по обща стойност):");
        foreach (var a in filtered)
        {
            Console.WriteLine($"{a.Number}, {a.Name}, {a.Price:F2} лв., {a.Quantity} броя, Обща стойност: {a.TotalValue():F2} лв.");
        }
    }

    static void ReportType4()
    {
        var type4 = artikuli.Where(a => a.Type == 4).ToList();

        if (type4.Count == 0)
        {
            Console.WriteLine("Няма въведени артикули от тип 4.");
            return;
        }

        double avgPrice = type4.Average(a => a.Price);
        double totalValue = type4.Sum(a => a.TotalValue());

        Console.WriteLine($"\nСправка за тип 4:");
        Console.WriteLine($"Средна единична цена: {avgPrice:F2} лв.");
        Console.WriteLine($"Обща стойност: {totalValue:F2} лв.");
    }
}
