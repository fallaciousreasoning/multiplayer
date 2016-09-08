using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Runner.Builders;

namespace Runner.Components
{
    public class CharacterInfo
    {
        /// <summary>
        /// Indicates whether the character was on the ground
        /// </summary>
        public bool WasOnGround;

        /// <summary>
        /// The direction the character is currently facing
        /// </summary>
        public Direction Facing;

        /// <summary>
        /// Indicates whether the character is moving
        /// </summary>
        public bool Moving;

        /// <summary>
        /// The current velocity of the character
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The amount of time till the player can next jump
        /// </summary>
        public float TillJump;

        /// <summary>
        /// Whether the character should roll on next contact with the ground
        /// </summary>
        public bool ShouldRoll;

        /// <summary>
        /// Gets the direction facing away from the wall (zero if neither or both sides are walls)
        /// </summary>
        public int AwayFromWall
        {
            get
            {
                var away = 0;
                if (LeftWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG)) away++;
                if (RightWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG)) away--;
                return away;
            }
        }

        /// <summary>
        /// Determines if the player can wall jump. You can wall jump if you are touching a wall, not touching the 
        /// ground and you can't clamber (clambering is preferred to wall jumping)
        /// </summary>
        public bool CanWallJump
            =>
                !OnGround &&
                (LeftWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG) ||
                 RightWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG)) && !CanClamber;

        /// <summary>
        /// Determines whether the player is on the ground
        /// </summary>
        public bool OnGround => GroundDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG);

        /// <summary>
        /// Indicates whether the player can stand
        /// </summary>
        public bool CanStand
            =>
                !CeilingDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG) &&
                !RoomToStandDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG);

        /// <summary>
        /// Determines whether the player can clamber onto a ledge. Ledge direction is 
        /// the opposite to wall jump direction.s
        /// </summary>
        public bool CanClamber
            =>
                (LeftWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG) ||
                 RightWallDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG)) &&
                !ClamberDetector.IsTouching(ObstacleBuilder.OBSTACLE_TAG);

        /// <summary>
        /// Detects when the gameobject is touching the ground
        /// </summary>
        public Touching GroundDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject touches the ceiling
        /// </summary>
        public Touching CeilingDetector { get; set; }

        /// <summary>
        /// A trigger placed a little above the gameobject to workout when we can stand
        /// </summary>
        public Touching RoomToStandDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touches a wall on the left
        /// </summary>
        public Touching LeftWallDetector { get; set; }

        /// <summary>
        /// Detects when the gameobject is touches a wall on the right
        /// </summary>
        public Touching RightWallDetector { get; set; }

        /// <summary>
        /// Detects when the player is not able to clamber
        /// </summary>
        public Touching ClamberDetector { get; set; }

        /// <summary>
        /// Indicates whether the character is touching an obstacle
        /// </summary>
        /// <param name="touching">The touching</param>
        /// <returns>Whether it is touching an obstacle</returns>
        public bool Triggered(Touching touching)
        {
            return touching.IsTouching(ObstacleBuilder.OBSTACLE_TAG);
        }
    }
}
