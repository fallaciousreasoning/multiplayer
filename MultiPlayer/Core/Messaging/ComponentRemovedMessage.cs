using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class ComponentRemovedMessage : ITargetedMessage
    {
        public ComponentRemovedMessage(Entity target, object component)
        {
            Target = target;
            Component = component;
        }

        public Entity Target { get; }
        public object Component { get; }
    }
}
