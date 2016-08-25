using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.Extensions
{
    public static class LinqExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action?.Invoke(item);
        }

        public static void AddAll<T>(this ObservableCollection<T> source, IEnumerable<T> items)
        {
            items.Foreach(source.Add);
        }

        public static void AddAll<T>(this ObservableCollection<T> source, Array items)
        {
            foreach (var item in items)
                source.Add((T) item);
        }
    }
}
