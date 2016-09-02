using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Animation.Messages
{
    public class PauseAnimationMessage : ITargetedMessage
    {
        public PauseAnimationMessage(Entity target)
        {
            Target = target;
        }

        public Entity Target { get; }
    }
}
