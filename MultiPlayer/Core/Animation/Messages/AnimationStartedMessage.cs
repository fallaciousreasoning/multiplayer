using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Animation.Messages
{
    public class AnimationStartedMessage : ITargetedMessage
    {
        public AnimationStartedMessage(Entity target, AnimationContainer container, Animation animation)
        {
            Target = target;
            Animation = animation;
            Container = container;
        }

        public Entity Target { get; }
        public Animation Animation { get; }
        public AnimationContainer Container { get; }
    }
}
