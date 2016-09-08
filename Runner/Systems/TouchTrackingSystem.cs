using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(CollisionMessage))]
    public class TouchTrackingSystem : ISystem
    {
        public void RecieveMessage(IMessage message)
        {
            if (!(message is CollisionMessage)) return;

            var collisionMessage = (CollisionMessage) message;

            if (!collisionMessage.IsTrigger || 
                !collisionMessage.Target.HasComponent<Touching>() ||
                !collisionMessage.Hit.HasComponent<Tag>()) return;

            var touchInfo = collisionMessage.Target.Get<Touching>();
            var hitTag = collisionMessage.Hit.Get<Tag>();

            if (collisionMessage.Mode == CollisionMode.Entered)
                touchInfo.Touched(hitTag.Tags);
            else
                touchInfo.Seperated(hitTag.Tags);
        }
    }
}
