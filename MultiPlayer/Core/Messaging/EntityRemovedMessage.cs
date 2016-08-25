using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class EntityRemovedMessage : ITargetedMessage
    {
        public EntityRemovedMessage(Entity entity)
        {
            Target = entity;
        }

        public Entity Target { get; }
    }
}
