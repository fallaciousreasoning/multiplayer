using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiPlayer.Serialization
{
    internal class SerializationContractResolver : DefaultContractResolver
    {
        private readonly HashSet<Entity> allowedEntities;
        private readonly HashSet<object> allowedComponents;

        internal SerializationContractResolver(HashSet<Entity> allowedEntities, HashSet<object> allowedComponents)
        {
            this.allowedEntities = allowedEntities;
            this.allowedComponents = allowedComponents;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = ShouldSerialize;
            return property;
        }

        private bool ShouldSerialize(object o)
        {
            var type = o.GetType();
            if (type.IsValueType)
            {
                return true;
            }
            if (type == typeof(Entity))
            {
                return allowedEntities.Contains((Entity)o);
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return true;
            }

            return allowedComponents.Contains(o);
        }
    }
}
