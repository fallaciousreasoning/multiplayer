using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Core.Messaging;

namespace XamlEditor.Scene.Messages
{
    public class EnableDraggerMessage : ITargetedMessage
    {
        public EnableDraggerMessage(Entity target, bool state)
        {
            Target = target;
            State = state;
        }

        public bool State { get; }
        public Entity Target { get; }
    }
}
