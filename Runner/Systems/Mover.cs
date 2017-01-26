using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    public class Mover : SimpleSystem<(CharacterStats stats, CharacterInput input, CharacterInfo info, Move move)>
    {
        public override void Update(Entity entity, (CharacterStats stats, CharacterInput input, CharacterInfo info, Move move) node)
        {
            var input = node.input;
            var stats = node.stats;
            var info = node.info;
            var step = Engine.Time.Step;

            if (input.Jump)
            {
                Jump(entity, stats, info);
                input.Jump = false;
            }

            if (input.Slide)
            {
                Slide(entity, stats, info);
                input.Slide = false;
            }

            var newVelocity = info.Velocity;

            var dir = MathHelper.Clamp(input.Direction, -1, 1);

            newVelocity.X += (info.OnGround ? stats.HorizontalAcceleration : stats.HorizontalAirAcceleration) * dir * step;

            info.Velocity = newVelocity;

            if (info.Triggered(info.LeftWallDetector) && info.Velocity.X < 0) info.Velocity.X = 0;
            if (info.Triggered(info.RightWallDetector) && info.Velocity.X > 0) info.Velocity.X = 0;
            if (info.Triggered(info.GroundDetector) && info.Velocity.Y > 0) info.Velocity.Y = 0;
            if (info.Triggered(info.CeilingDetector) && info.Velocity.Y < 0) info.Velocity.Y = 0;

            if (dir == 0)
                info.Velocity.X -= info.Velocity.X*(info.OnGround ? stats.HorizontalDrag : stats.HorizontalAirDrag)*step;
            
            if (!info.WasOnGround && info.OnGround && info.ShouldRoll)
            {
                info.ShouldRoll = false;
                entity.Remove<Move>();
                entity.Add<Roll>();
            }

            info.WasOnGround = info.OnGround;
        }

        private void Jump(Entity entity, CharacterStats stats, CharacterInfo info)
        {
            if (info.TillJump > 0) return;

            if (info.OnGround)
            {
                info.Velocity.Y = stats.JumpImpulse;
            }
            else if (info.CanClamber)
            {
                info.Velocity = Vector2.Zero;
                entity.Remove<Move>();
                entity.Add<Clamber>();
            }
            else if (info.CanWallJump)
            {
                info.Velocity.X = stats.WallJumpHorizontalImpulse*info.AwayFromWall*10;
                info.Velocity.Y = stats.WallJumpVerticalImpulse;
            }

            info.TillJump = stats.JumpDelay;
        }

        private void Slide(Entity entity, CharacterStats stats, CharacterInfo info)
        {
            if (info.OnGround)
            {
                entity.Remove<Move>();
                entity.Add<Slide>();

                info.TillJump = stats.SlideJumpDelay;
            }
            else info.ShouldRoll = true;
        }
    }
}
