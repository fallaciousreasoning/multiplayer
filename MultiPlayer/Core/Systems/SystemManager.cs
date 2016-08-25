using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Systems;

namespace MultiPlayer.Core
{
    public class SystemManager : IEnumerable<ISystem>
    {
        private readonly Dictionary<Type, ISystem> systemsMap = new Dictionary<Type, ISystem>();
        private readonly LinkedList<ISystem> systems = new LinkedList<ISystem>();

        public void Add(ISystem system)
        {
            systemsMap.Add(system.GetType(), system);
            var node = systems.AddLast(system);
            if (system is IKnowsNode<ISystem>)
                (system as IKnowsNode<ISystem>).Node = node;
        }

        public void Remove(ISystem system)
        {
            systemsMap.Remove(system.GetType());

            if (system is IKnowsNode<ISystem>)
                systems.Remove((system as IKnowsNode<ISystem>).Node);
            else systems.Remove(system);
        }

        public T Get<T>()
            where T : ISystem
        {
            return (T)Get(typeof(T));
        }

        public ISystem Get(Type t)
        {
            return systemsMap[t];
        }

        public IEnumerator<ISystem> GetEnumerator()
        {
            return systems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return systems.GetEnumerator();
        }
    }
}
