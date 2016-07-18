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
        /// The direction the character is currently facing
        /// </summary>
        public Direction Facing { get; private set; }

        /// <summary>
        /// The stats that describe how the character moves
        /// </summary>
        public CharacterStats Stats { get; set; } = new CharacterStats();

        /// <summary>
        /// The state of the character motor
        /// </summary>
        public CharacterState State { get; private set; } = CharacterState.Normal;

        /// <summary>
        /// Gets the direction facing away from the wall (zero if neither or both sides are walls)
        /// </summary>
        public int AwayFromWall
        {
            get
            {
                var away = 0;
                if (LeftWallDetector.Triggered) away++;
                if (RightWallDetector.Triggered) away--;
                return away;
            }
        }

        /// <summary>
        /// Determines if the player can wall jump. You can wall jump if you are touching a wall, not touching the 
        /// ground and you can't clamber (clambering is preferred to wall jumping)
        /// </summary>
        public bool CanWallJump
        {
            get { return !OnGround && (LeftWallDetector.Triggered || RightWallDetector.Triggered) && !CanClamber; }
        }

        /// <summary>
        /// Indicates whether the character is moving
        /// </summary>
        public bool Moving { get { return Stats.VelocityMinForMoving*Stats.VelocityMinForMoving > Velocity.LengthSquared(); } }

        /// <summary>
        /// Determines whether the player is on the ground
        /// </summary>
        public bool OnGround { get { return GroundDetector.Triggered; } }

        /// <summary>
        /// Determines whether the player can clamber onto a ledge. Ledge direction is 
        /// the opposite to wall jump direction.s
        /// </summary>
        public bool CanClamber
        {
            get { return (LeftWallDetector.Triggered || RightWallDetector.Triggered) && !ClamberDetector.Triggered; }
        }

        public Vector2 Velocity;

        private float tillJump;


        private bool shouldRoll;

        private Collider collider;

        private int dir;
        private bool wasOnGround;

        private Animation currentPhysicsAnimation;

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



            collider.Body.LinearVelocity = newVelocity;

            if (LeftWallDetector.Triggered && newVelocity.X < 0) newVelocity.X = 0;
            if (RightWallDetector.Triggered && newVelocity.X > 0) newVelocity.X = 0;
            if (GroundDetector.Triggered && newVelocity.Y > 0) newVelocity.Y = 0;
            if (CeilingDetector.Triggered && newVelocity.Y < 0)
                newVelocity.Y = 0;

            Velocity = newVelocity;
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
                    //State = CharacterState.Rolling;
                    shouldRoll = false;

                    Animator.Start(Animations.Name(PlayerAnimation.Roll, Facing));
                }
            }

            wasOnGround = OnGround;
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
            //TODO rolling
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
        /// Detects when the gameobject is touching the ground
        /// </summary>
        public TriggerDetector GroundDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject touches the ceiling
        /// </summary>
        public TriggerDetector CeilingDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touches a wall on the left
        /// </summary>
        public TriggerDetector LeftWallDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touches a wall on the right
        /// </summary>
        public TriggerDetector RightWallDetector { get; set; }

        /// <summary>
        /// Detects when the player is not able to clamber
        /// </summary>
        public TriggerDetector ClamberDetector { get; set; }

        /// <summary>
        /// The animator for the game object
        /// </summary>
        public AnimationController Animator { get; set; }
    }
}
