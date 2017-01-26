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
        T NodeForEntity(Entity entity);
        Entity EntityForNode(T node);
    }

    public class NodeFamily<T> : INodeFamily<T>
    {
        private const string TUPLE_NAME = "ValueTuple";

        public IObservableLinkedList UntypedNodes => Nodes;
        public IObservableLinkedList<T> Nodes { get; } = new ObservableLinkedList<T>();

        public ConstituentTypes ConstituentTypes { get; }

        private readonly ObjectActivator<T> activator;

        private readonly Type type;

        private readonly ISet<FieldInfo> fieldSet = new HashSet<FieldInfo>();
        private readonly List<FieldInfo> fieldTypes = new List<FieldInfo>();

        private readonly Dictionary<Entity, LinkedListNode<T>> entityNodeMap = new Dictionary<Entity, LinkedListNode<T>>();
        private readonly Dictionary<T, Entity> nodeEntityMap = new Dictionary<T, Entity>();

        private readonly bool isTuple;
        private readonly Type[] genericTypeArguments;

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
            isTuple = type.Name.StartsWith(TUPLE_NAME);
            if (isTuple)
            {
                genericTypeArguments = type.GenericTypeArguments;
            }
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
            nodeEntityMap.Add(item, entity);

            var node = new LinkedListNode<T>(item);

            entityNodeMap.Add(entity, node);
            Nodes.AddLast(node);
        }

        private void Remove(Entity entity)
        {
            var node = entityNodeMap[entity];
            Nodes.Remove(node);

            entityNodeMap.Remove(entity);
            nodeEntityMap.Remove(node.Value);
        }

        private T CreateFromEntity(Entity entity)
        {
            //Special handling for tuples
            object[] args;
            if (isTuple)
            {
                args = new object[genericTypeArguments.Length];

                for (var i = 0; i < genericTypeArguments.Length; ++i)
                    args[i] = entity.Get(genericTypeArguments[i]);
            }
            else args = new object[0];

            var instance = activator(args);

            var canLoad = instance as ICanLoad<T>;
            canLoad?.LoadFrom(entity);

            //If the object knows how to load itself, do that.
            if (canLoad != null) return instance;

            //Otherwise use reflection
            foreach (var field in fieldTypes)
                field.SetValue(instance, entity.Get(field.FieldType));

            return instance;
        }

        public T NodeForEntity(Entity entity)
        {
            return entityNodeMap.ContainsKey(entity) ? entityNodeMap[entity].Value : CreateFromEntity(entity);
        }

        public Entity EntityForNode(T node)
        {
            return nodeEntityMap[node];
        }
    }
}
