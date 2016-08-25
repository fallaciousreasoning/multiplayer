using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core
{
    public class ReadOnlyEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> collection;

        public ReadOnlyEnumerable(IEnumerable<T> collection)
        {
            this.collection = collection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
