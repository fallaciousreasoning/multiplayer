using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;
using MultiPlayer.Reflection;

namespace MultiPlayer.Core.Families
{
    public interface INodeFamily
    {
        IObservableLinkedList UntypedNodes { get; }
        ConstituentTypes ConstituentTypes { get; }

        void Register(FamilyManager familyManager);
    }

    public interface INodeFamily<T> : INodeFamily
    {
        IObservableLinkedList<T> Nodes { get; }
        T NodeFromEntity(Entity entity);
    }

    public class NodeFamily<T> : INodeFamily<T>
    {
        public IObservableLinkedList UntypedNodes => Nodes;
        public IObservableLinkedList<T> Nodes { get; } = new ObservableLinkedList<T>();

        public ConstituentTypes ConstituentTypes { get; }

        private readonly ObjectActivator<T> activator;

        private readonly Type type;

        private readonly ISet<FieldInfo> fieldSet = new HashSet<FieldInfo>();
        private readonly List<FieldInfo> fieldTypes = new List<FieldInfo>();

        private readonly Dictionary<Entity, LinkedListNode<T>> entityNodeMap = new Dictionary<Entity, LinkedListNode<T>>();

        public NodeFamily()
        {
            type = typeof(T);
            activator = ObjectActivatorHelpers.GetActivator<T>();

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic || !field.IsPublic) continue;

                fieldTypes.Add(field);
                fieldSet.Add(field);
            }

            ConstituentTypes = new ConstituentTypes(fieldSet.Select(f => f.FieldType));
        }

        public void Register(FamilyManager familyManager)
        {
            var relevantFamily = familyManager.Get(ConstituentTypes);
            relevantFamily.Entities.ItemAdded += (source, item) => Add((Entity)item);
            relevantFamily.Entities.ItemRemoved += (source, item) => Remove((Entity)item);
        }

        private void Add(Entity entity)
        {
            var item = CreateFromEntity(entity);
            var node = Nodes.AddLast(item);
            entityNodeMap.Add(entity, node);
        }

        private void Remove(Entity entity)
        {
            var node = entityNodeMap[entity];
            Nodes.Remove(node);

            entityNodeMap.Remove(entity);
        }

        private T CreateFromEntity(Entity entity)
        {
            var instance = activator();

            var canLoad = instance as ICanLoad<T>;
            canLoad?.LoadFrom(entity);

            //If the object knows how to load itself, do that.
            if (canLoad != null) return instance;

            //Otherwise use reflection
            foreach (var field in fieldTypes)
                field.SetValue(instance, entity.Get(field.FieldType));

            return instance;
        }

        public T NodeFromEntity(Entity entity)
        {
            return CreateFromEntity(entity);
        }
    }
}
