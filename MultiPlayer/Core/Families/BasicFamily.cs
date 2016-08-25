using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Families
{
    public class BasicFamily : IFamily
    {
        protected readonly List<Entity> entities = new List<Entity>();
        protected readonly HashSet<Type> ConstituentTypes = new HashSet<Type>();

        public IEnumerable<Entity> Entities => entities;

        protected BasicFamily(IEnumerable<Type> constituentTypes)
        {
            constituentTypes.Foreach(c => ConstituentTypes.Add(c));
        }

        public void OnEntityCreated(Entity entity)
        {
            if (ConstituentTypes.All(entity.HasComponent))
                entities.Add(entity);
        }

        public void OnEntityRemoved(Entity entity)
        {
            if (!ConstituentTypes.All(entity.HasComponent))
                entities.Remove(entity);
        }

        public void OnComponentAdded(Entity entity, object component)
        {
            var type = component.GetType();
            if (!ConstituentTypes.Contains(type)) return;

            if (ConstituentTypes.All(entity.HasComponent) && !entities.Contains(entity))
                entities.Add(entity);
        }

        public void OnComponentRemoved(Entity entity, object component)
        {
            if (ConstituentTypes.Contains(component.GetType()))
                entities.Remove(entity);
        }
    }
}
