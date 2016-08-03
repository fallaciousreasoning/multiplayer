using System;
using System.Collections.Generic;
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
    }
}
