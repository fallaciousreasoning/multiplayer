using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Animation.Messages
{
    public class StopAnimationMessage : IMessage
    {
        public StopAnimationMessage(Entity target)
        {
            Target = target;
        }

        public Entity Target { get; }
    }
}
