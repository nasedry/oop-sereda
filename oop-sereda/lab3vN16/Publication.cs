using System;

namespace lab3vN16
{
    public class Publication
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Publication(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public virtual void GetInfo()
        {
            Console.WriteLine($"Назва: {Title}, Автор: {Author}");
        }
    }
}
