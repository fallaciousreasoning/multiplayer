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
    public class Mover : ComponentProcessingSystem<CharacterStats, CharacterInput, CharacterInfo, Move>
    {
        protected override void Process(IMessage message, CharacterStats stats, CharacterInput input, CharacterInfo info, Move move)
        {
            var updateMessage = message as UpdateMessage;
            if (updateMessage != null)
                Update(updateMessage.Time.Step, stats, input,info);
        }

        private void Update(float step, CharacterStats stats, CharacterInput input, CharacterInfo info)
        {
            if (input.Jump)
            {
                Jump(stats, info);
                input.Jump = false;
            }

            if (input.Slide)
            {
                Slide(stats, info);
                input.Slide = false;
            }

            var newVelocity = info.Velocity;

            var dir = MathHelper.Clamp(input.Direction, -1, 1);

            newVelocity.X += (info.OnGround ? stats.HorizontalAcceleration : stats.HorizontalAirAcceleration) * dir * step;
            newVelocity += stats.Gravity*step;

            info.Velocity = newVelocity;

            if (dir == 0)
                info.Velocity.X -= info.Velocity.X*(info.OnGround ? stats.HorizontalDrag : stats.HorizontalAirDrag)*step;

            info.TillJump -= step;

            if (!info.WasOnGround && info.OnGround && info.ShouldRoll)
            {
                //TODO Roll
            }

            info.WasOnGround = info.OnGround;
        }

        private void Jump(CharacterStats stats, CharacterInfo info)
        {
            if (info.TillJump > 0) return;

            if (info.OnGround)
            {
                info.Velocity.Y = stats.JumpImpulse;
            }
            else if (info.CanClamber)
            {
                info.Velocity = Vector2.Zero;
                //TODO clamber
            }
            else if (info.CanWallJump)
            {
                info.Velocity.X = stats.WallJumpHorizontalImpulse*info.AwayFromWall*10;
                info.Velocity.Y = stats.WallJumpHorizontalImpulse;
            }

            info.TillJump = stats.JumpDelay;
        }

        private void Slide(CharacterStats stats, CharacterInfo info)
        {
            if (info.OnGround)
            {
                //TODO slide
                info.TillJump = stats.SlideJumpDelay;
            }
            else info.ShouldRoll = true;
        }
    }
}
