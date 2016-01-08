using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Physics
{
    public class PhysicsWorld : IUpdateable
    {
        public Vector2 Gravity { get { return World.Gravity; } set { World.Gravity = value; } }
        public World World { get; private set; }

        public PhysicsWorld()
        {
            World = new World(Vector2.Zero);
        }

        /// <summary>
        /// Removes a body from the world
        /// </summary>
        /// <param name="collider">The collider to remove</param>
        public void Remove(Collider collider)
        {
            World.RemoveBody(collider.Body);
        }

        public void Update(float step)
        {
            World.BodyList.ForEach(b =>
            {
                var transform = (b.UserData as GameObject)?.Transform;
                if (transform == null) return;

                if (transform.Position != b.Position || transform.Rotation != b.Rotation)
                {
                    b.Awake = true;
                    b.Position = transform.Position;
                    b.Rotation = transform.Rotation;
                }
            });

            World.Step(step);

            World.BodyList.ForEach(b =>
            {
                var transform = (b.UserData as GameObject)?.Transform;
                if (transform == null) return;

                transform.Position = b.Position;
                transform.Rotation = b.Rotation;
            });
        }
    }
}
