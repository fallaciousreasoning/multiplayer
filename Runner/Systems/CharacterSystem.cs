using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;
using CharacterStats = Runner.Builders.CharacterStats;

namespace Runner.Systems
{
    public class CharacterSystem : ComponentProcessingSystem<CharacterStats, CharacterInput, CharacterInfo>
    {
        protected override void Process(IMessage message, CharacterStats stats, CharacterInput input, CharacterInfo info)
        {
            info.Moving = info.Velocity.LengthSquared() > stats.VelocityMinForMoving;

            if (info.Velocity.X < -stats.VelocityMinForMoving)
                info.Facing = Direction.Left;

            if (info.Velocity.X > stats.VelocityMinForMoving)
                info.Facing = Direction.Right;
        }
    }
}
