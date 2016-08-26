using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;

namespace MultiPlayer.Core.Families
{
    public class SophisticatedFamily<T> : IFamily<T> where T : class, new()
    {
        private readonly ObservableLinkedList<T> nodes = new ObservableLinkedList<T>();

        private readonly Dictionary<Entity, LinkedListNode<T>> entityMap = new Dictionary<Entity, LinkedListNode<T>>();

        private readonly List<FieldInfo> constituentFields = new List<FieldInfo>();
        private readonly List<Type> constituentTypes = new List<Type>();
        private readonly HashSet<Type> constituentSet = new HashSet<Type>();

        private ConstructorInfo constructor;
        
        public SophisticatedFamily()
        {
            Initialize();
        }

        private void Initialize()
        {
            var type = typeof(T);

            constructor = type.GetConstructor(new Type[0]);

            var fields = type.GetFields();

            foreach (var field in fields)
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic || !field.IsPublic) continue;

                constituentFields.Add(field);

                var fieldType = field.FieldType;
                constituentSet.Add(fieldType);
                constituentTypes.Add(fieldType);
            }
        }


        public void OnEntityCreated(Entity entity)
        {
            AddIfMatch(entity);
        }

        public void OnEntityRemoved(Entity entity)
        {
            Remove(entity);
        }

        public void OnComponentAdded(Entity entity, object component)
        {
            if (!constituentSet.Contains(component.GetType())) return;

            AddIfMatch(entity);
        }

        public void OnComponentRemoved(Entity entity, object component)
        {
            //If the component isn't important, ignore it
            if (!constituentSet.Contains(component.GetType())) return;

            //If it was, remove it
            Remove(entity);
        }

        private void AddIfMatch(Entity entity)
        {
            //If we already have this entity, don't do anything
            if (entityMap.ContainsKey(entity)) return;

            //If this entity doesn't meet our requirements, don't do anything
            if (!Matches(entity)) return;

            var item = CreateFromEntity(entity);
            var node = nodes.AddLast(item);
            entityMap.Add(entity, node);
        }

        private T CreateFromEntity(Entity entity)
        {
            var node = (T)constructor.Invoke(new object[0]);

            var canLoad = node as ICanLoad<T>;
            canLoad?.LoadFrom(entity);

            if (canLoad != null) return node;

            foreach (var fieldInfo in constituentFields)
                fieldInfo.SetValue(node, entity.Get(fieldInfo.FieldType));

            return node;
        }

        private void Remove(Entity entity)
        {
            if (!entityMap.ContainsKey(entity)) return;

            var node = entityMap[entity];
            entityMap.Remove(entity);

            nodes.Remove(node);
        }

        private bool Matches(Entity entity)
        {
            return constituentTypes.All(entity.HasComponent);
        }

        public IObservableLinkedList<T> Nodes => nodes;
        public IObservableLinkedList UntypedNodes => Nodes;
    }
}
