using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public class DiveInitiator : ComponentProcessingSystem<Divable>
    {
        protected override void Process(IMessage message, Divable component)
        {
            if (!(message is UpdateMessage)) return;
            if (component.Characters == null) return;

            foreach (var character in component.Characters)
            {
                //Make sure the character is in the right state
                if (!character.HasComponent<Move>() || !character.HasComponent<CharacterInfo>()) continue;

                var info = character.Get<CharacterInfo>();

                //Make sure we're going fast enough
                if (Math.Abs(info.Velocity.X) < component.MinSpeedForDive || info.Velocity.Y > - 3) continue;

                //Transition to the dive state
                character.Remove<Move>();
                character.Add<Dive>();
            }
        }
    }
}
