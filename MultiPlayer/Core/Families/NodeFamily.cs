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
    public class NodeFamily<T>
    {
        public ObservableLinkedList<T> Nodes { get; } = new ObservableLinkedList<T>();
        public ImmutableHashSet<Type> ConstituentTypes { get; }

        private readonly ObjectActivator<T> activator;

        private readonly Type type;

        private readonly ISet<FieldInfo> fieldSet = new HashSet<FieldInfo>();
        private readonly List<FieldInfo> fieldTypes = new List<FieldInfo>();

        public NodeFamily()
        {
            type = typeof(T);
            activator = ObjectActivatorHelpers.GetActivator<T>();

            Initialize();
        }

        private void Initialize()
        {
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic || !field.IsPublic) continue;
                
                fieldTypes.Add(field);
                fieldSet.Add(field);
            }
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
    }
}
