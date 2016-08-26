using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public enum CollisionMode { Entered, Exited }
    public class CollisionMessage : ITargetedMessage
    {
        public CollisionMessage(Entity target, Entity hit, bool isTrigger, CollisionMode mode)
        {
            Target = target;
            Hit = hit;

            IsTrigger = isTrigger;
            Mode = mode;
        }

        public CollisionMode Mode { get; }
        public bool IsTrigger { get; }

        public Entity Hit { get; }

        public Entity Target { get; }
    }
}
