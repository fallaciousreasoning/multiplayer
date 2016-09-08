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
    public class CharacterSystem : ComponentProcessingSystem<Collider, CharacterStats, CharacterInput, CharacterInfo>
    {
        protected override void Process(IMessage message, Collider collider, CharacterStats stats, CharacterInput input, CharacterInfo info)
        {
            var updateMessage = message as UpdateMessage;
            if (updateMessage == null) return;

            var step = updateMessage.Time.Step;

            info.Velocity += stats.Gravity * step;

            info.Velocity.X = MathHelper.Clamp(info.Velocity.X, -stats.MaxXSpeed, stats.MaxXSpeed);
            info.Velocity.Y = MathHelper.Clamp(info.Velocity.Y, -stats.MaxYSpeed, stats.MaxYSpeed);

            collider.Velocity = info.Velocity;

            info.Moving = info.Velocity.LengthSquared() > stats.VelocityMinForMoving;

            if (info.Velocity.X < -stats.VelocityMinForMoving)
                info.Facing = Direction.Left;

            if (info.Velocity.X > stats.VelocityMinForMoving)
                info.Facing = Direction.Right;

            info.TillJump -= step;
        }
    }
}
