using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace MultiPlayer.GameComponents.Physics
{
    public static class ColliderFactory
    {
        public static Collider BoxCollider(float width, float height, BodyType bodyType = BodyType.Kinematic, bool fixedRotation = false)
        {
            var collider = new Collider()
            {
                Body = BodyFactory.CreateRectangle(Game1.Game.PhysicsWorld.World, width, height, 1),
                BodyType = bodyType
            };
            collider.Body.Friction = 0f;
            collider.Body.FixedRotation = fixedRotation;
            return collider;
        }

        public static Collider BoxTrigger(float width, float height)
        {
            return new Collider()
            {
                Body = BodyFactory.CreateRectangle(Game1.Game.PhysicsWorld.World, width, height, 1),
                BodyType = BodyType.Dynamic,
                IsTrigger = true
            };
        }
    }
}
