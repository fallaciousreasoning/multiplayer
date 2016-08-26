using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;

namespace MultiPlayer.Core.Families
{
    public interface IFamily
    {
        IObservableLinkedList UntypedNodes { get; }

        void OnEntityCreated(Entity entity);
        void OnEntityRemoved(Entity entity);
        void OnComponentAdded(Entity entity, object component);
        void OnComponentRemoved(Entity entity, object component);
    }

    public interface IFamily<T> : IFamily
    {
        IObservableLinkedList<T> Nodes { get; }
    }
}
