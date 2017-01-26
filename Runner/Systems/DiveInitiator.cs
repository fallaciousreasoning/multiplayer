using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    public class DiveInitiator : SimpleSystem<Divable>
    {
        public override void Update(Entity entity, Divable node)
        {
            base.Update(entity, node);

            if (node.Characters == null) return;

            foreach (var character in node.Characters)
            {
                //Make sure the character is in the right state
                if (!character.HasComponent<Move>() || !character.HasComponent<CharacterInfo>()) continue;

                var info = character.Get<CharacterInfo>();

                //Make sure we're going fast enough
                if (Math.Abs(info.Velocity.X) < node.MinSpeedForDive || info.Velocity.Y > -3) continue;

                //Transition to the dive state
                character.Remove<Move>();
                character.Add<Dive>();
            }
        }
    }
}
