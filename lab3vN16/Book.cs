using System;

namespace lab3vN16
{
    public class Book : Publication
    {
        public int Pages { get; set; }
        public Book(string title, string author, int pages)
            : base(title, author)
        {
            Pages = pages;
        }

        public override void GetInfo()
        {
            Console.WriteLine($"[Книга] Назва: {Title}, Автор: {Author}, Сторінок: {Pages}");
        }
    }
}
