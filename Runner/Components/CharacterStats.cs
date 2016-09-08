using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Runner.Components
{
    public class CharacterStats
    {
        /// <summary>
        /// The Horizontal impulse applied to the player, away from the wall when wall jumping
        /// </summary>
        public float WallJumpHorizontalImpulse { get; set; } = 5;

        /// <summary>
        /// The vertical impulse applied to the player when wall jumping
        /// </summary>
        public float WallJumpVerticalImpulse { get; set; } = -10;

        /// <summary>
        /// The minimum velocity the character needs to maintain to be counted as 'moving'
        /// </summary>
        public float VelocityMinForMoving { get; set; } = 0.01f;

        /// <summary>
        /// The minimum delay for the character to jump after sliding
        /// </summary>
        public float SlideJumpDelay { get; set; } = 0.1f;

        /// <summary>
        /// The minimum delay between jumps
        /// </summary>
        public float JumpDelay { get; set; } = 0.2f;

        /// <summary>
        /// The vertical impulse applied to the character when jumping
        /// </summary>
        public float JumpImpulse { get; set; } = -10;

        /// <summary>
        /// The horizontal acceleration applied to the character when in the air
        /// </summary>
        public float HorizontalAirAcceleration { get; set; } = 50;

        /// <summary>
        /// The horizontal acceleration applied to the character on the ground
        /// </summary>
        public float HorizontalAcceleration { get; set; } = 100;

        /// <summary>
        /// The minimum horizontal velocity the character needs to slide
        /// </summary>
        public float MinSlideVelocity { get; set; } = 3;

        /// <summary>
        /// The maximum horizontal speed of the character
        /// </summary>
        public float MaxXSpeed { get; set; } = 10;

        /// <summary>
        /// The maximum vertical speed of the character
        /// </summary>
        public float MaxYSpeed { get; set; } = 10;

        /// <summary>
        /// The horizontal drag applied to the character when in the air
        /// </summary>
        public float HorizontalAirDrag { get; set; } = 0.5f;

        /// <summary>
        /// The horizontal drage applied to the character when on the ground
        /// </summary>
        public float HorizontalDrag { get; set; } = 5;

        /// <summary>
        /// The horizontal drag applied to the character when sliding
        /// </summary>
        public float HorizontalSlideDrag { get; set; } = 1f;

        /// <summary>
        /// The acceleration applied to the character due to gravity
        /// </summary>
        public Vector2 Gravity { get; set; } = new Vector2(0, 15);
    }
}
