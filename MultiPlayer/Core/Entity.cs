using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core
{
    public class Entity : IKnowsNode<Entity>
    {
        private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();

        public void Add<T>(T component)
            where T : class
        {
            components.Add(typeof(T), component);
            ComponentAdded?.Invoke(this, component);
        }

        public void Remove<T>(T component)
            where T : class
        {
            components.Remove(typeof(T));
            ComponentRemoved?.Invoke(this, component);
        }

        public void Clear()
        {
            components.Clear();
        }

        public T Get<T>()
            where T : class
        {
            var type = typeof(T);
            return components.ContainsKey(type) ? (T)components[type] : null;
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        internal bool HasComponent(Type type)
        {
            return components.ContainsKey(type);
        }

        public Action<Entity, object> ComponentAdded;
        public Action<Entity, object> ComponentRemoved;

        public int Id { get; internal set; }
        public LinkedListNode<Entity> Node { get; set; }
        public Engine Engine { get; set; }
    }
}
