using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Extensions;
using Newtonsoft.Json;

namespace MultiPlayer.Serialization
{
    public class EntitySerializer
    {
        public string Serialize(Entity entity)
        {
            var allowedComponents = new HashSet<object>();
            var allowedEntities = new HashSet<Entity>();

            AddAllowed(entity, allowedEntities, allowedComponents);
            
            var contractResolver = new SerializationContractResolver(allowedEntities, allowedComponents);

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = contractResolver,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var result = JsonConvert.SerializeObject(entity, settings);
            return result;
        }

        public Entity Deserialize(string json)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            };
            var result = JsonConvert.DeserializeObject<Entity>(json);
            return result;
        }

        private void AddAllowed(Entity e, HashSet<Entity> allowedEntities, HashSet<object> allowedComponents)
        {
            allowedEntities.Add(e);
            e.Components.ForEach(c => allowedComponents.Add(c));

            var children = e.Get<HasChildren>();
            children?.Children.ForEach(c => AddAllowed(c, allowedEntities, allowedComponents));
        }
    }
}
