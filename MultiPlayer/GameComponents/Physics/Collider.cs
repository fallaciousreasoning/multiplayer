using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Physics
{
    public class Collider : ILateUpdateable, IKnowsGameObject, IStartable, IHearsDestroy
    {
        private Body body;
        private bool isTrigger;

        public Body Body
        {
            get { return body; }
            internal set
            {
                if (Body != null) throw new InvalidOperationException("Body cannot be changed!");

                body = value;
            }
        }

        public bool IsTrigger
        {
            get { return isTrigger; }
            set
            {
                if (IsTrigger == value) return;

                isTrigger = value;
                Body.IsSensor = value;
            }
        }

        public BodyType BodyType
        {
            get { return Body.BodyType; }
            set { Body.BodyType = value; }
        }

        private Vector2 lastScale = Vector2.One;

        internal Collider()
        {
            
        }

        public void LateUpdate(float step)
        {
            if (lastScale != GameObject.Transform.Scale)
            {
                foreach (var fixture in Body.FixtureList)
                {
                    switch (fixture.Shape.ShapeType)
                    {
                        case ShapeType.Polygon:
                            var polygon = fixture.Shape as PolygonShape;
                            polygon.Vertices.Scale(GameObject.Transform.Scale/lastScale);
                            break;
                        case ShapeType.Chain:
                            var chain = fixture.Shape as ChainShape;
                            chain.Vertices.Scale(GameObject.Transform.Scale/lastScale);
                            break;
                        case ShapeType.Circle:
                            var circle = fixture.Shape as CircleShape;
                            circle.Radius *= (GameObject.Transform.Scale.Length()/lastScale.Length());
                            break;
                        default:
                            throw new Exception("Unsupported shape type!");
                    }
                }
            }
            lastScale = GameObject.Transform.Scale;
        }

        public GameObject GameObject { get; set; }

        public void Start()
        {
            if (Body == null) throw new InvalidOperationException("Body must be set before the collider can be started!");
            
            Body.Position = GameObject.Transform.Position;
            Body.Rotation = GameObject.Transform.Rotation;

            Body.UserData = GameObject;
            Body.OnCollision += OnCollided;
            Body.OnSeparation += OnSeperated;
        }

        private bool OnCollided(Fixture a, Fixture b, Contact contact)
        {
            var hit = a.Body == Body ? b.Body : a.Body;
            var hitObject = hit.UserData as GameObject;
            if (IsTrigger) 
                GameObject.HearTriggers.ForEach(t => t.OnTriggerEntered(hitObject));
            else GameObject.HearCollisions.ForEach(t => t.OnCollisionEnter(hitObject));

            return true;
        }

        private void OnSeperated(Fixture a, Fixture b)
        {
            var hit = a.Body == Body ? b.Body : a.Body;
            var hitObject = hit.UserData as GameObject;
            if (IsTrigger)
                GameObject.HearTriggers.ForEach(t => t.OnTriggerExited(hitObject));
            else GameObject.HearCollisions.ForEach(t => t.OnCollisionExit(hitObject));
        }

        public void OnDestroy()
        {
            Game1.Game.PhysicsWorld.Remove(this);

            Body.OnCollision -= OnCollided;
            Body.OnSeparation -= OnSeperated;
        }
    }
}
