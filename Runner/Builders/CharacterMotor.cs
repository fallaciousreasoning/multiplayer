using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Physics;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Runner.Builders
{
    public class CharacterMotor : IUpdateable, IStartable, IKnowsGameObject
    {
        /// <summary>
        /// An animation of the player clambering over a ledge to the right
        /// </summary>
        public AnimationController ClamberRightAnimation;

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
        /// The Horizontal impulse applied to the player, away from the wall when wall jumping
        /// </summary>
        public float WallJumpHorizontalImpulse = 5;

        /// <summary>
        /// The vertical impulse applied to the player when wall jumping
        /// </summary>
        public float WallJumpVerticalImpulse = -10;

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

        public float JumpDelay = 0.2f;
        private float tillJump;

        public float JumpImpulse = -10;
        public Vector2 Velocity;

        public float HorizontalAirAcceleration { get; set; } = 50;
        public float HorizontalAcceleration { get; set; } = 100;
        public float HorizontalSlideAcceleration { get; set; } = 0;

        public float MaxXSpeed { get; set; } = 10;
        public float MaxYSpeed { get; set; } = 10;

        public float HorizontalAirDrag { get; set; } = 0.5f;
        public float HorizontalDrag { get; set; } = 5;
        public float HorizontalSlideDrag { get; set; } = 1f;

        public Vector2 Gravity { get; set; } = new Vector2(0, 15);

        private Collider collider;

        private int dir;
        private bool wasOnGround;

        private AnimationController currentAnimationController;

        public void Start()
        {
            collider = GameObject.GetComponent<Collider>();
        }

        public void Update(float step)
        {
            if (currentAnimationController != null)
            {
                if (!currentAnimationController.IsPlaying)
                    currentAnimationController = null;
                return;
            }

            var newVelocity = Velocity;

            dir = MathHelper.Clamp(dir, -1, 1);
            newVelocity.X += GetHorizontalAcceleration()*dir*step;

            //Add the acceleration due to gravity
            newVelocity += Gravity*step;

            //Clamp the x velocity to a maximum
            newVelocity.X = MathHelper.Clamp(newVelocity.X, -MaxXSpeed, MaxXSpeed);
            newVelocity.Y = MathHelper.Clamp(newVelocity.Y, -MaxYSpeed, MaxYSpeed);

            if (LeftWallDetector.Triggered && Velocity.X < 0) Velocity.X = 0;
            if (RightWallDetector.Triggered && Velocity.X > 0) Velocity.X = 0;
            if (GroundDetector.Triggered && Velocity.Y > 0) Velocity.Y = 0;

            collider.Body.LinearVelocity = newVelocity;

            Velocity = newVelocity;
            if (dir == 0)
                Velocity.X -= Velocity.X*GetHorizontalDrag()*step;

            dir = 0;
            tillJump -= step;

            wasOnGround = OnGround;
        }

        /// <summary>
        /// Tells the motor to accelerate left
        /// </summary>
        public void AccelerateRight()
        {
            dir++;
        }
        
        /// <summary>
        /// Tells the player to accelerate right
        /// </summary>
        public void AccelerateLeft()
        {
            dir--;
        }

        /// <summary>
        /// Tells the player to jump
        /// </summary>
        public void Jump()
        {
            if (tillJump > 0)
                return;

            //Jump
            if (OnGround)
            {
                Velocity.Y = JumpImpulse;
            }
            else if (CanClamber)
            {
                Velocity = Vector2.Zero;
                currentAnimationController = ClamberRightAnimation;
                currentAnimationController.Start();
            }
            else if (CanWallJump)
            {
                Velocity.X = WallJumpHorizontalImpulse*AwayFromWall*10;
                Velocity.Y = WallJumpVerticalImpulse;
            }

            tillJump = JumpDelay;
        }

        /// <summary>
        /// Tells the player to slide
        /// </summary>
        public void Slide()
        {
            throw new NotImplementedException();
        }

        private float GetHorizontalAcceleration()
        {
            return OnGround ? HorizontalAcceleration : HorizontalAirAcceleration;
        }

        private float GetHorizontalDrag()
        {
            return OnGround ? HorizontalDrag : HorizontalAirDrag;
        }

        public GameObject GameObject { get; set; }

        /// <summary>
        /// Detects when the gameobject is touching the ground
        /// </summary>
        public TriggerDetector GroundDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touch a wall on the left
        /// </summary>
        public TriggerDetector LeftWallDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touch a wall on the right
        /// </summary>
        public TriggerDetector RightWallDetector { get; set; }

        /// <summary>
        /// Detects when the player is not able to clamber
        /// </summary>
        public TriggerDetector ClamberDetector { get; set; }
    }
}
