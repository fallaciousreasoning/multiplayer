using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner.Components
{
    public class CharacterInfo
    {
        public bool OnGround;
        public bool WasOnGround;

        /// <summary>
        /// The direction the character is currently facing
        /// </summary>
        public Direction Facing;

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
        public bool Moving { get { return Stats.VelocityMinForMoving * Stats.VelocityMinForMoving > Velocity.LengthSquared(); } }

        /// <summary>
        /// Determines whether the player is on the ground
        /// </summary>
        public bool OnGround { get { return GroundDetector.Triggered; } }

        /// <summary>
        /// Indicates whether the player can stand
        /// </summary>
        public bool CanStand { get { return !CeilingDetector.Triggered && !RoomToStandDetector.Triggered; } }

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

        /// <summary>
        /// Detects when the gameobject is touching the ground
        /// </summary>
        public TriggerDetector GroundDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject touches the ceiling
        /// </summary>
        public TriggerDetector CeilingDetector { get; set; }

        /// <summary>
        /// A trigger placed a little above the gameobject to workout when we can stand
        /// </summary>
        public TriggerDetector RoomToStandDetector { get; set; }

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
    }
}
