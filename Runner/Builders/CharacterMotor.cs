using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Animation;
using MultiPlayer.GameComponents.Physics;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Runner.Builders
{
    [Flags]
    public enum CharacterState
    {
        Normal = 1,
        Sliding = 2,
        OnWall = 4,
        Clambering = 8,
        Rolling = 16
    }

    public class CharacterMotor : IUpdateable, IStartable, IKnowsGameObject
    {

        /// <summary>
        /// The state of the character motor
        /// </summary>
        public CharacterState State { get; internal set; } = CharacterState.Normal;

        

        

        
        

        
        

        

        public void Start()
        {
            collider = GameObject.GetComponent<Collider>();
        }

        public void Update(float step)
        {
            if (currentPhysicsAnimation != null)
            {
                if (!currentPhysicsAnimation.IsPlaying)
                    currentPhysicsAnimation = null;
                return;
            }

            var newVelocity = Velocity;

            dir = MathHelper.Clamp(dir, -1, 1);
            newVelocity.X += GetHorizontalAcceleration()*dir*step;

            //Add the acceleration due to gravity
            newVelocity += Stats.Gravity*step;

            //Clamp the x velocity to a maximum
            newVelocity.X = MathHelper.Clamp(newVelocity.X, -Stats.MaxXSpeed, Stats.MaxXSpeed);
            newVelocity.Y = MathHelper.Clamp(newVelocity.Y, -Stats.MaxYSpeed, Stats.MaxYSpeed);

            Velocity = newVelocity;

            ManageSlide();

            collider.Body.LinearVelocity = Velocity;

            //Hitting stuff
            if (State != CharacterState.Sliding)
            {
                if (LeftWallDetector.Triggered && Velocity.X < 0) Velocity.X = 0;
                if (RightWallDetector.Triggered && Velocity.X > 0) Velocity.X = 0;
                if (GroundDetector.Triggered && Velocity.Y > 0) Velocity.Y = 0;
                if (CeilingDetector.Triggered && Velocity.Y < 0) Velocity.Y = 0;
            }

            //Drag
            if (dir == 0)
                Velocity.X -= Velocity.X*GetHorizontalDrag()*step;

            if (Velocity.X < -Stats.VelocityMinForMoving)
                Facing = Direction.Left;
            if (Velocity.X > Stats.VelocityMinForMoving)
                Facing = Direction.Right;

            dir = 0;
            tillJump -= step;

            //If we've just hit the ground
            if (!wasOnGround && OnGround)
            {
                if (shouldRoll)
                {
                    Roll();
                }
            }

            wasOnGround = OnGround;
        }

        private void ManageSlide()
        {
            if (State != CharacterState.Sliding) return;

            var d = (Facing == Direction.Left ? -1 : 1);

            //If we aren't going fast enough
            if (Math.Abs(Velocity.X) - Stats.MinSlideVelocity < 0)
            {
                if (CanStand) SlideJump();
                else
                {
                    Velocity.X = d*Stats.MinSlideVelocity;
                    
                }
            } 
        }

        /// <summary>
        /// Tells the motor to accelerate left
        /// </summary>
        public void AccelerateRight()
        {
            if (State == CharacterState.Sliding) return;

            dir++;
        }
        
        /// <summary>
        /// Tells the player to accelerate right
        /// </summary>
        public void AccelerateLeft()
        {
            if (State == CharacterState.Sliding) return;

            dir--;
        }

        /// <summary>
        /// Tells the player to jump
        /// </summary>
        public void Jump()
        {
            switch (State)
            {
                case CharacterState.Sliding:
                    SlideJump();
                    break;
                default:
                    NormalJump();
                    break;
            }

            shouldRoll = false;
        }

        /// <summary>
        /// Tells the character to execute a roll
        /// </summary>
        public void Roll()
        {
            State = CharacterState.Rolling;
            shouldRoll = false;

            Animator.Start(Animations.Name(PlayerAnimation.Roll, Facing));
        }

        private void SlideJump()
        {
            if (Animator.IsPlaying) return;

            Animator.Start(Animations.Name(PlayerAnimation.SlideUp, Facing));
            State = CharacterState.Normal;
            tillJump = 0.5f;
        }

        private void NormalJump()
        {
            if (tillJump > 0)
                return;

            //Jump
            if (OnGround)
            {
                Velocity.Y = Stats.JumpImpulse;
            }
            else if (CanClamber)
            {
                Velocity = Vector2.Zero;
                currentPhysicsAnimation = Animator.Start(Animations.Name(PlayerAnimation.Clamber, Facing));
            }
            else if (CanWallJump)
            {
                Velocity.X = Stats.WallJumpHorizontalImpulse * AwayFromWall * 10;
                Velocity.Y = Stats.WallJumpVerticalImpulse;
            }

            tillJump = Stats.JumpDelay;
        }

        /// <summary>
        /// Tells the player to slide
        /// </summary>
        public void Slide()
        {
            if (!State.HasFlag(CharacterState.Normal)) return;

            if (OnGround)
            {
                currentPhysicsAnimation = Animator.Start(Animations.Name(PlayerAnimation.SlideDown, Facing));
                State = CharacterState.Sliding;
                tillJump = Stats.SlideJumpDelay;
            }
            else
            {
                shouldRoll = true;
            }
        }

        /// <summary>
        /// Calculates the horizontal acceleration to apply
        /// </summary>
        /// <returns>The acceleration</returns>
        private float GetHorizontalAcceleration()
        {
            return OnGround ? Stats.HorizontalAcceleration : Stats.HorizontalAirAcceleration;
        }

        /// <summary>
        /// Calculates the horizontal drag to apply
        /// </summary>
        /// <returns>The drag</returns>
        private float GetHorizontalDrag()
        {
            return State == CharacterState.Sliding
                ? Stats.HorizontalSlideDrag
                : OnGround
                    ? Stats.HorizontalDrag
                    : Stats.HorizontalAirDrag;
        }

        public GameObject GameObject { get; set; }

        

        /// <summary>
        /// The animator for the game object
        /// </summary>
        public AnimationController Animator { get; set; }
    }
}
