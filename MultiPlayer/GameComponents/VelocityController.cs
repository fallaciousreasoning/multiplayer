using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class VelocityController : IUpdateable, IKnowsGameObject
    {
        public float MaxSpeed = 10;
        public Vector2 Velocity;

        public float AngularVelocity;
        public float MaxAngularVelocity = 4;

        public void Update(float step)
        {
            //Clamp our velocity
            if (Velocity.LengthSquared() > MaxSpeed*MaxSpeed)
                Velocity = Vector2.Normalize(Velocity)*MaxSpeed;

            GameObject.Transform.LocalPosition += Velocity*step;

            AngularVelocity = MathHelper.Clamp(AngularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
            GameObject.Transform.LocalRotation -= AngularVelocity*step;
        }

        public GameObject GameObject { get; set; }
    }
}
