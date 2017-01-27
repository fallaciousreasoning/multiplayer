using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;

namespace MultiPlayer.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IObservableLinkedList<T> source, Action<T> action)
        {
            var node = source.First;
            while (node != null)
            {
                action(node.Value);
                node = node.Next;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action.Invoke(item);
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
