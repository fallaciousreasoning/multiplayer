using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Core.Messaging;

namespace XamlEditor.Scene.Messages
{
    public class ComponentChangedMessage : ITargetedMessage
    {
        public ComponentChangedMessage(Entity target, object component)
        {
            Component = component;
            Target = target;
        }

        public Entity Target { get; }
        public object Component { get; }
    }
}
