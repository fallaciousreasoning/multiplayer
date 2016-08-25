using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Families
{
    public class FamilyManager
    {
        private readonly Dictionary<Type, IFamily> familyTypes = new Dictionary<Type, IFamily>();
        private readonly LinkedList<IFamily> families = new LinkedList<IFamily>();

        public void Register<T>()
            where T : IFamily, new()
        {
            var family = new T();
            familyTypes.Add(typeof(T), family);
            families.AddLast(family);
        }

        internal void Register(Type type)
        {
            if (!typeof(IFamily).IsAssignableFrom(type)) throw new ArgumentException($"{type.Name} does not implement IFamily!");
            var constructor = type.GetConstructor(new Type[0]);
            if (constructor == null) throw new ArgumentException($"{type.Name} does not contain a default constructor!");

            var instance = (IFamily)Activator.CreateInstance(type);
            familyTypes.Add(type, instance);
            families.AddLast(instance);
        }

        public void OnEntityCreated(Entity entity)
        {
            families.Foreach(f => f.OnEntityCreated(entity));
        }

        public void OnEntityRemoved(Entity entity)
        {
            families.Foreach(f => f.OnEntityRemoved(entity));
        }

        public void OnComponentAdded(Entity entity, object component)
        {
            families.Foreach(f => f.OnComponentAdded(entity, component));
        }

        public void OnComponentRemoved(Entity entity, object component)
        {
            families.Foreach(f => f.OnComponentRemoved(entity, component));
        }

        public T Get<T>()
            where T : IFamily
        {
            return (T) Get(typeof(T));
        }

        public IFamily Get(Type type)
        {
            return familyTypes[type];
        }
    }
}
