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
        public bool OnGround { get { return GroundDetector.OnGround; } }

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
            if (!OnGround) return;

           Velocity += JumpImpulse;
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
        public GroundDetector GroundDetector { get; set; }
    }
}
