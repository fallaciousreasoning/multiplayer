using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(AnimationFinishedMessage))]
    public class Clamberer : SimpleSystem<(CharacterStats stats, CharacterInfo info, AnimationContainer container, Clamber clamber)>
    {
        public override void OnNodeAdded(Entity entity, (CharacterStats stats, CharacterInfo info, AnimationContainer container, Clamber clamber) node)
        {
            base.OnNodeAdded(entity, node);

            var startAnimationMessage = new StartAnimationMessage(entity,
                Animations.Name(PlayerAnimation.Clamber, entity.Get<CharacterInfo>().Facing));
            Engine.MessageHub.SendMessage(startAnimationMessage);
        }

        public override void OnAnimationFinished(Entity entity, (CharacterStats stats, CharacterInfo info, AnimationContainer container, Clamber clamber) node)
        {
            base.OnAnimationFinished(entity, node);

            foreach (var otherNode in Nodes)
            {
                var otherEntity = NodeFamily.EntityForNode(otherNode);
                if (entity != otherEntity) continue;

                entity.Remove<Clamber>();
                entity.Add<Move>();
                break;
            }
        }
    }
}
