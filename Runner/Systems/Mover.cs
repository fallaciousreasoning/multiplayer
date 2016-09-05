using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    public class Mover : ComponentProcessingSystem<Collider, CharacterStats, CharacterInput, CharacterInfo, Move>
    {
        protected override void Process(IMessage message, Collider collider, CharacterStats stats, CharacterInput input, CharacterInfo info, Move move)
        {
        }
    }
}
