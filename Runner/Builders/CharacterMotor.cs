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
        /// Gets the direction facing away from the wall (zero if neither or both sides are walls)
        /// </summary>
        public int AwayFromWall
        {
            get
            {
                var away = 0;
                if (LeftWallDetector.Triggered) away--;
                if (RightWallDetector.Triggered) away++;
                return away;
            }
        }

        /// <summary>
        /// Determines if the player can wall jump
        /// </summary>
        public bool CanWallJump
        {
            get { return !OnGround && (LeftWallDetector.Triggered || RightWallDetector.Triggered); }
        }

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

        public Vector2 JumpImpulse { get; set; } = new Vector2(0, -5);
        public Vector2 Velocity;

        public float HorizontalAirAcceleration { get; set; } = 50;
        public float HorizontalAcceleration { get; set; } = 100;
        public float MaxXSpeed { get; set; } = 10;
        public float MaxYSpeed { get; set; } = 10;

        public float HorizontalAirDrag { get; set; } = 0.5f;
        public float HorizontalDrag { get; set; } = 5;

        public Vector2 Gravity { get; set; } = new Vector2(0, 10);

        private Collider collider;

        private int dir;

        public void Start()
        {
            collider = GameObject.GetComponent<Collider>();
        }

        public void Update(float step)
        {
            var newVelocity = Velocity;

            dir = MathHelper.Clamp(dir, -1, 1);
            newVelocity.X += GetHorizontalAcceleration()*dir*step;

            //Add the acceleration due to gravity
            newVelocity += Gravity*step;

            //Clamp the x velocity to a maximum
            newVelocity.X = MathHelper.Clamp(newVelocity.X, -MaxXSpeed, MaxXSpeed);
            newVelocity.Y = MathHelper.Clamp(newVelocity.Y, -MaxYSpeed, MaxYSpeed);

            collider.Body.LinearVelocity = newVelocity;

            Velocity = newVelocity;
            if (dir == 0)
                Velocity.X -= Velocity.X*GetHorizontalDrag()*step;

            dir = 0;
        }

        public void AccelerateRight()
        {
            dir++;
        }

        public void AccelerateLeft()
        {
            dir--;
        }

        public void Jump()
        {
            //Jump
            if (OnGround)
            {
                Velocity += JumpImpulse;
            }
            else if (CanWallJump)
            {
                
            }
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
