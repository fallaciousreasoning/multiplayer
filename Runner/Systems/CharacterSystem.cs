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
    public class CharacterSystem : SimpleSystem<(Collider collider, CharacterStats stats, CharacterInput input, CharacterInfo info)>
    {
        public override void Update((Collider collider, CharacterStats stats, CharacterInput input, CharacterInfo info) node)
        {
            var step = Engine.Time.Step;

            node.info.Velocity += node.stats.Gravity * step;

            node.info.Velocity.X = MathHelper.Clamp(node.info.Velocity.X, -node.stats.MaxXSpeed, node.stats.MaxXSpeed);
            node.info.Velocity.Y = MathHelper.Clamp(node.info.Velocity.Y, -node.stats.MaxYSpeed, node.stats.MaxYSpeed);

            node.collider.Velocity = node.info.Velocity;

            node.info.Moving = node.info.Velocity.LengthSquared() > node.stats.VelocityMinForMoving;

            if (node.info.Velocity.X < -node.stats.VelocityMinForMoving)
                node.info.Facing = Direction.Left;

            if (node.info.Velocity.X > node.stats.VelocityMinForMoving)
                node.info.Facing = Direction.Right;

            node.info.TillJump -= step;
        }
    }
}
