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

        public void Add<T>()
            where T : class, new()
        {
            Add(new T());
        }

        public void Add(object component)
        {
            var type = component.GetType();

            components.Add(type, component);
            ComponentAdded?.Invoke(this, component);
        }

        public void Remove<T>()
            where T : class
        {
            var component = components[typeof(T)];
            Remove(component);
        }

        public void Remove(Type t)
        {
            var component = components[t];
            Remove(component);
        }

        public void Remove(object component)
        {
            components.Remove(component.GetType());
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
            return (T)Get(type);
        }

        public object Get(Type t)
        {
            return components.ContainsKey(t) ? components[t] : null;
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type type)
        {
            return components.ContainsKey(type);
        }

        public Action<Entity, object> ComponentAdded;
        public Action<Entity, object> ComponentRemoved;
        
        public int Id { get; internal set; }
        public string FriendlyName { get; set; }
        public LinkedListNode<Entity> Node { get; set; }
        public Engine Engine { get; set; }
        public IEnumerable<object> Components => components.Values;
    }
}
