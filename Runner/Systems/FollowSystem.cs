using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public class FollowSystem : ComponentProcessingSystem<Transform, Follow>
    {
        protected override void Process(IMessage message, Transform transform, Follow follow)
        {
            var update = message as UpdateMessage;
            if (update == null) return;

            transform.Position = Vector2.Lerp(transform.Position, follow.Target.Position,
                follow.Spring*update.Time.Step);
        }
    }
}
