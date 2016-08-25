using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Families
{
    public interface IFamily
    {
        IEnumerable<Entity> Entities { get; }

        void OnEntityCreated(Entity entity);
        void OnEntityRemoved(Entity entity);
        void OnComponentAdded(Entity entity, object component);
        void OnComponentRemoved(Entity entity, object component);
    }
}
