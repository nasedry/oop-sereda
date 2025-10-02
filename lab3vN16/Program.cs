using System;
using System.Collections.Generic;

namespace lab3vN16
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Publication> publications = new List<Publication>
            {
                new Book("Червоне і чорне", "Стендаль", 480),
                new Magazine("Наука і життя", "О. Іваненко", 10),
                new Book("Майстер і Маргарита", "М. Булгаков", 390),
                new Magazine("Forbes", "Редакція Forbes", 40)
            };

            foreach (var pub in publications)
            {
                pub.GetInfo();
            }
        }
    }
}

