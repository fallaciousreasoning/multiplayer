using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Collections
{
    public delegate void LinkedListChangeEventHandler(IObservableLinkedList source, object item);

    public interface IObservableLinkedList : IEnumerable
    {
        event LinkedListChangeEventHandler ItemAdded;
        event LinkedListChangeEventHandler ItemRemoved;

        object AddFirst(object item);
        object AddLast(object item);
        void RemoveFirst();
        void RemoveLast();
        void Remove(object item);
        void Clear();
        int Count { get; }
    }

    public interface IObservableLinkedList<T> : IEnumerable<T>, IObservableLinkedList
    {
        LinkedListNode<T> AddFirst(T item);
        LinkedListNode<T> AddLast(T item);

        void Remove(T item);
        void Remove(LinkedListNode<T> item);
    }

    public class ObservableLinkedList<T> : IObservableLinkedList<T>
    {
        public event LinkedListChangeEventHandler ItemAdded;
        public event LinkedListChangeEventHandler ItemRemoved;

        protected LinkedList<T> Items = new LinkedList<T>();

        public LinkedListNode<T> First => Items.First;
        public LinkedListNode<T> Last => Items.Last;

        public LinkedListNode<T> AddFirst(T item)
        {
            var node = Items.AddFirst(item);

            ItemAdded?.Invoke(this,item);
            return node;
        }

        public LinkedListNode<T> AddLast(T item)
        {
            var node = Items.AddLast(item);

            ItemAdded?.Invoke(this, item);
            return node;
        }

        public object AddFirst(object item)
        {
            return AddFirst((T)item);
        }

        public object AddLast(object item)
        {
            return AddLast((T)item);
        }

        public void RemoveFirst()
        {
            var node = Items.First;
            Items.RemoveFirst();

            ItemRemoved?.Invoke(this, node.Value);
        }

        public void RemoveLast()
        {
            var node = Items.Last;
            Items.RemoveLast();

            ItemRemoved?.Invoke(this, node.Value);
        }

        public void Remove(object item)
        {
            if (item is LinkedListNode<T>)
                Remove((LinkedListNode<T>) item);
            else if (item is T)
                Remove((T)item);

            ItemRemoved?.Invoke(this, item);
        }

        public void Remove(T item)
        {
            var node = Items.Find(item);
            if (node == null) return;
            
            Items.Remove(node);

            ItemRemoved?.Invoke(this, node.Value);
        }

        public void Remove(LinkedListNode<T> node)
        {
            Items.Remove(node);

            ItemRemoved?.Invoke(this, node.Value);
        }

        public void Clear()
        {
            while (First != null)
                RemoveFirst();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Items.Count;
    }
}
