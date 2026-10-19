using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryProject
{
    public class LibraryItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public bool IsAvailable { get; set; } = true; // Статус доступності

        public LibraryItem(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public virtual string GetInfo()
        {
            return $"[ID: {Id}] Назва: '{Title}'";
        }
    }

    public class Book : LibraryItem
    {
        public string Author { get; private set; }
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

        public Book(int id, string title, string author, int pages) : base(id, title)
        {
            Author = author;
            Pages = pages;
        }

        public override string GetInfo() => $"КНИГА -> {base.GetInfo()}, Автор: {Author}, Сторінок: {Pages}";
    }

    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; private set; }

        public Magazine(int id, string title, int issueNumber) : base(id, title)
        {
            IssueNumber = issueNumber;
        }

        public override string GetInfo() => $"ЖУРНАЛ -> {base.GetInfo()}, Випуск №{IssueNumber}";
    }

    public class Reader
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Loan
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ReaderId { get; set; }
        public DateTime IssueDate { get; set; }
    }

    public class LibraryService
    {
        private List<LibraryItem> _items = new List<LibraryItem>();
        private List<Reader> _readers = new List<Reader>();
        private List<Loan> _loans = new List<Loan>();
        private int _loanIdCounter = 1;

        public void AddItem(LibraryItem item) => _items.Add(item);
        public void AddReader(Reader reader) => _readers.Add(reader);

        public List<LibraryItem> GetAllAvailableItems()
        {
            return _items.Where(i => i.IsAvailable).ToList();
        }

        public void BorrowItem(int readerId, int itemId)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == readerId);
            var item = _items.FirstOrDefault(i => i.Id == itemId);

            if (reader == null)
                throw new Exception("Читача з таким ID не знайдено.");
            if (item == null)
                throw new Exception("Матеріал з таким ID не знайдено.");

            if (!item.IsAvailable)
                throw new Exception("Цей матеріал вже виданий іншому читачу!");

            item.IsAvailable = false;
            _loans.Add(new Loan
            {
                Id = _loanIdCounter++,
                ItemId = item.Id,
                ReaderId = reader.Id,
                IssueDate = DateTime.Now
            });
        }
    }

    class Program
    {
        static void Main()
        {
            
            LibraryService service = new LibraryService();

            
            service.AddItem(new Book(1, "1984", "Джордж Орвелл", 328));
            service.AddItem(new Book(2, "Кобзар", "Тарас Шевченко", 720));
            service.AddItem(new Magazine(3, "Forbes Україна", 12));

            service.AddReader(new Reader { Id = 1, Name = "Іван Франко" });

            while (true)
            {
                Console.WriteLine("\n=== МЕНЮ БІБЛІОТЕКИ ===");
                Console.WriteLine("1. Показати всі доступні матеріали");
                Console.WriteLine("2. Оформити видачу матеріалу читачу");
                Console.WriteLine("0. Вийти");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var availableItems = service.GetAllAvailableItems();
                            Console.WriteLine("\n--- ДОСТУПНІ МАТЕРІАЛИ ---");
                            foreach (var item in availableItems)
                            {
                                
                                Console.WriteLine(item.GetInfo());
                            }
                            break;

                        case "2":
                            Console.Write("\nВведіть ID читача (наприклад, 1): ");
                            int readerId = int.Parse(Console.ReadLine());

                            Console.Write("Введіть ID матеріалу (книги/журналу): ");
                            int itemId = int.Parse(Console.ReadLine());

                            
                            service.BorrowItem(readerId, itemId);
                            Console.WriteLine("Успіх! Матеріал видано.");
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("ПОМИЛКА: Введіть коректне число!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ПОМИЛКА БІЗНЕС-ЛОГІКИ: {ex.Message}");
                }
            }
        }
    }
}