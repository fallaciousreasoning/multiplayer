using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using XamlEditor.Scene.Components;

namespace XamlEditor.Scene.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public class StickToSystem : ComponentProcessingSystem<StickTo, Transform>
    {
        protected override void Process(IMessage message, StickTo stick, Transform transform)
        {
            if (message is UpdateMessage)
            {
                var to = stick.To.Get<Transform>();
                transform.Position = to.Position;
                transform.Rotation = to.Rotation;
                transform.Scale = to.Scale;
            }
        }
    }
}
