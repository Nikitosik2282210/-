using System;
using System.Collections.Generic;


public class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; }

    public virtual string GetInfo()
    {
        return $"[ID: {Id}] Назва: '{Title}'";
    }
}

public class Book : LibraryItem
{
    public string Author { get; set; }

    private int _pages;
    public int Pages
    {
        get { return _pages; }
        set
        {
            if (value <= 0) throw new ArgumentException("Кількість сторінок має бути більшою за 0.");
            _pages = value;
        }
    }

    public override string GetInfo() => $"КНИГА -> {base.GetInfo()}, Автор: {Author}, Стор: {Pages}";
}

public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }

    public override string GetInfo() => $"ЖУРНАЛ -> {base.GetInfo()}, Випуск №{IssueNumber}";
}

class Program
{
    static void Main()
    {
        List<LibraryItem> inventory = new List<LibraryItem>
        {
            new Book
            {
                Id = 1,
                Title = "1984",
                Author = "Джордж Орвелл",
                Pages = 328
            },
            new Book
            {
                Id = 2,
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Pages = 720
            },
            new Magazine
            {
                Id = 3,
                Title = "Forbes Україна",
                IssueNumber = 12
            }
        };

        Console.WriteLine("--- ІНВЕНТАР БІБЛІОТЕКИ ---\n");

        foreach (var item in inventory)
        {
            Console.WriteLine(item.GetInfo());
        }
    }
}