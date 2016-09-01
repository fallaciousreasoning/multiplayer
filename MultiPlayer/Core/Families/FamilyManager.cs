using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;
using MultiPlayer.Reflection;

namespace MultiPlayer.Core.Families
{
    public class FamilyManager
    {
        private readonly Dictionary<Type, INodeFamily> nodeFamilyTypes = new Dictionary<Type, INodeFamily>();
        private readonly LinkedList<INodeFamily> nodeFamilies = new LinkedList<INodeFamily>();

        private readonly Dictionary<ISet<Type>, IFamily> familyTypes = new Dictionary<ISet<Type>, IFamily>();
        private readonly LinkedList<IFamily> families = new LinkedList<IFamily>();

        private readonly ObjectActivator<SophisticatedFamily> defaultFamilyActivator = ObjectActivatorHelpers.GetActivator<SophisticatedFamily>();
        private readonly Type nodeType = typeof(NodeFamily<>);

        internal void Register(ISet<Type> types)
        {
            if (familyTypes.ContainsKey(types)) return;

            var instance = defaultFamilyActivator();
            familyTypes.Add(types, instance);
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

        public IFamily Get(ISet<Type> constituentTypes)
        {
            return familyTypes[constituentTypes];
        }

        public void RegisterNodeFamily(Type type)
        {
            if (nodeFamilyTypes.ContainsKey(type)) return;

            var genericNodeType = nodeType.MakeGenericType(type);
            var nodeFamily = (INodeFamily)Activator.CreateInstance(genericNodeType);

            nodeFamilies.AddLast(nodeFamily);
            nodeFamilyTypes.Add(type, nodeFamily);

            nodeFamily.Register(this);
        }

        public void RegisterNodeFamily<T>()
        {
            var type = typeof(T);
            if (nodeFamilyTypes.ContainsKey(type)) return;

            var nodeFamily = new NodeFamily<T>();

            nodeFamilies.AddLast(nodeFamily);
            nodeFamilyTypes.Add(type, nodeFamily);

            nodeFamily.Register(this);
        }

        public INodeFamily<T> GetNodeFamily<T>()
        {
            return (INodeFamily<T>)nodeFamilyTypes[typeof(T)];
        }
    }
}
