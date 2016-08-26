using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class StartAnimationMessage : ITargetedMessage
    {
        public StartAnimationMessage(Entity target, string animation)
        {
            Animation = animation;
            Target = target;
        }

        public Entity Target { get; }
        public string Animation { get; }
    }
}
