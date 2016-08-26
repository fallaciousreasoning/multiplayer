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
    public class ColliderBuilder
    {
        private Vertices vertices;
        private bool isTrigger;
        private bool fixedRotation;
        private BodyType bodyType = BodyType.Kinematic;
        private float density = 1;

        private ColliderBuilder()
        {
            
        }

        public static ColliderBuilder New() { return new ColliderBuilder().BoxShape(1,1); }

        public ColliderBuilder AtDensity(float density)
        {
            this.density = density;
            return this;
        }

        public ColliderBuilder IsFixedRotation(bool fixedRotation)
        {
            this.fixedRotation = fixedRotation;
            return this;
        }

        public ColliderBuilder IsTrigger(bool isTrigger = true)
        {
            this.isTrigger = isTrigger;
            return this;
        }

        public ColliderBuilder BoxShape(float width, float height)
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

        public ColliderBuilder IsDynamic()
        {
            return WithBodyType(BodyType.Dynamic);
        }
        public ColliderBuilder IsStatic()
        {
            return WithBodyType(BodyType.Static);
        }

        public ColliderBuilder IsKinematic()
        {
            return WithBodyType(BodyType.Kinematic);
        }

        public ColliderBuilder WithBodyType(BodyType bodyType)
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
