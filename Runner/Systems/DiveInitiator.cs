using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Builders;
using Runner.Components;

namespace Runner.Systems
{
    public class DiveInitiator : SimpleSystem<(Divable divable, Transform transform)>
    {
        public override void Update(Entity entity, (Divable divable, Transform transform) node)
        {
            base.Update(entity, node);

            var divable = node.divable;
            if (divable.Characters == null) return;

            foreach (var character in divable.Characters)
            {
                //Make sure the character is in the right state
                if (!character.HasComponent<Move>() || !character.HasComponent<CharacterInfo>()) continue;

                var info = character.Get<CharacterInfo>();

                //Make sure we're going fast enough
                if (Math.Abs(info.Velocity.X) < divable.MinSpeedForDive || info.Velocity.Y > -3) continue;

                //Transition to the dive state
                Engine.MessageHub.SendMessage(new StateTransitionMessage(character, CharacterPrefab.DIVE_STATE));
            }
        }
    }
}
