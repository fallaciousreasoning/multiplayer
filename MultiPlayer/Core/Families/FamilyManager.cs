using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Families
{
    public class FamilyManager
    {
        private readonly Dictionary<Type, IFamily> familyTypes = new Dictionary<Type, IFamily>();
        private readonly LinkedList<IFamily> families = new LinkedList<IFamily>();

        private readonly Type defaultFamilyType=typeof(SophisticatedFamily<>);

        public void Register<T>()
            where T : class, new()
        {
            var type = typeof(T);
            if (familyTypes.ContainsKey(type)) return;

            var family = new SophisticatedFamily<T>();
            familyTypes.Add(type, family);
            families.AddLast(family);
        }

        internal void Register(Type type)
        {
            if (familyTypes.ContainsKey(type)) return;

            var genericType = defaultFamilyType.MakeGenericType(type);

            var instance = (IFamily)Activator.CreateInstance(genericType);
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

        public IFamily<T> Get<T>()
            where T : class, new()
        {
            return (IFamily<T>) Get(typeof(T));
        }

        public IFamily Get(Type type)
        {
            return familyTypes[type];
        }
    }
}
