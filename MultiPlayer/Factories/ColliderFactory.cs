using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Factories
{
    public class ColliderFactory
    {
        private Vertices vertices;
        private bool isTrigger;
        private bool fixedRotation;
        private BodyType bodyType = BodyType.Kinematic;
        private float density = 1;

        private ColliderFactory()
        {
            
        }

        public static ColliderFactory New() { return new ColliderFactory().BoxShape(1,1); }

        public ColliderFactory AtDensity(float density)
        {
            this.density = density;
            return this;
        }

        public ColliderFactory IsFixedRotation(bool fixedRotation)
        {
            this.fixedRotation = fixedRotation;
            return this;
        }

        public ColliderFactory IsTrigger(bool isTrigger = true)
        {
            this.isTrigger = isTrigger;
            return this;
        }

        public ColliderFactory BoxShape(float width, float height)
        {
            var halfWidth = width*0.5f;
            var halfHeight = height*0.5f;

            var points = new[]
            {
                -new Vector2(halfWidth, halfHeight),
                new Vector2(halfWidth, - halfHeight),
                new Vector2(halfWidth, halfHeight),
                new Vector2(-halfWidth, halfHeight)
            };
            vertices = new Vertices(points);
            vertices.ForceCounterClockWise();

            return this;
        }

        public ColliderFactory IsDynamic()
        {
            return WithBodyType(BodyType.Dynamic);
        }
        public ColliderFactory IsStatic()
        {
            return WithBodyType(BodyType.Static);
        }

        public ColliderFactory IsKinematic()
        {
            return WithBodyType(BodyType.Kinematic);
        }

        public ColliderFactory WithBodyType(BodyType bodyType)
        {
            this.bodyType = bodyType;
            return this;
        }

        public Collider Create()
        {
            return new Collider()
            {
                BodyType = bodyType,
                IsTrigger = isTrigger,
                Shape = vertices,
                FixedRotation = fixedRotation,
                Density = density
            };
        }
    }
}
