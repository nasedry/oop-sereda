using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5v15.Generics
{
    public class Repository<T>
    {
        private List<T> _items = new List<T>();

        public void Add(T item) => _items.Add(item);
        public void Remove(T item) => _items.Remove(item);
        public IEnumerable<T> All() => _items.AsReadOnly();
        public IEnumerable<T> Where(Func<T, bool> predicate) => _items.Where(predicate);

        public static IEnumerable<T> TopN(IEnumerable<T> items, int n, Func<T, double> selector)
        {
            return items.OrderByDescending(selector).Take(n);
        }
    }
}
