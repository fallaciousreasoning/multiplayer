using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public class CollisionSystem : LateUpdatableSystem
    {
        public Vector2 Gravity
        {
            get { return world.Gravity; }
            set { world.Gravity = value; }
        }

        private readonly World world = new World(Vector2.Zero);

        public CollisionSystem() : base(new [] {typeof(CollidableFamily)})
        {
            CanReceive.Add(typeof(EntityAddedMessage));
            CanReceive.Add(typeof(EntityRemovedMessage));
        }

        protected override void Handle(IMessage message)
        {
            base.Handle(message);

            var addedMessage = message as EntityAddedMessage;
            if (addedMessage != null)
                OnEntityAdded(addedMessage.Target);

            var removedMessage = message as EntityRemovedMessage;
            if (removedMessage != null)
                OnEntityAdded(removedMessage.Target);
        }

        protected void OnEntityAdded(Entity entity)
        {
            if (!entity.HasComponent<Collider>()) return;

            var collider = entity.Get<Collider>();
            
            collider.Body = CreateBody(entity, collider);
            collider.Body.UserData = entity;
            collider.Body.OnCollision += OnCollided;
            collider.Body.OnSeparation += OnSeperated;
        }

        protected void OnEntityRemoved(Entity entity)
        {
            if (!entity.HasComponent<Collider>()) return;

            var collider = entity.Get<Collider>();

            collider.Body.OnCollision -= OnCollided;
            collider.Body.OnSeparation -= OnSeperated;

            world.RemoveBody(collider.Body);
        }

        protected override void LateUpdate(Time time)
        {
            world.BodyList.ForEach(b =>
            {
                var entity = (b.UserData as Entity);
                var transform = entity.Get<Transform>();
                var collider = entity.Get<Collider>();

                if (transform.Position != b.Position || transform.Rotation != b.Rotation)
                {
                    b.Awake = true;
                    b.Position = transform.Position;
                    b.Rotation = transform.Rotation;
                }

                if (transform.Scale != collider.LastScale)
                    Resize(transform, collider);

                if (!Equals(collider.BodyType, collider.Body.BodyType))
                    collider.Body.BodyType = collider.BodyType;

                if (collider.Body.FixedRotation != collider.FixedRotation)
                    collider.Body.FixedRotation = collider.FixedRotation;

                if (collider.WasTrigger != collider.IsTrigger)
                {
                    collider.Body.IsSensor = collider.IsTrigger;
                    collider.WasTrigger = collider.IsTrigger;
                }
            });

            world.Step(time.Step);

            world.BodyList.ForEach(b =>
            {
                var transform = (b.UserData as Entity).Get<Transform>();
                if (transform.Position != b.Position || transform.Rotation != b.Rotation)
                {
                    b.Awake = true;
                    transform.Position = b.Position;
                    transform.Rotation = b.Rotation;
                }
            });
        }

        private void Resize(Transform transform, Collider collider)
        {
            foreach (var fixture in collider.Body.FixtureList)
            {
                switch (fixture.Shape.ShapeType)
                {
                    case ShapeType.Polygon:
                        var polygon = fixture.Shape as PolygonShape;
                        polygon.Vertices.Scale(transform.Scale / collider.LastScale);
                        break;
                    default:
                        throw new Exception("Unsupported Shape Type!");
                }
            }
            collider.LastScale = transform.Scale;
        }

        protected bool OnCollided(Fixture a, Fixture b, Contact contact)
        {
            var target = (a.Body.UserData as Entity);
            var hit = (b.Body.UserData as Entity);

            var colliderA = target.Get<Collider>();

            Engine.MessageHub.SendMessage(new CollisionMessage(target, hit, colliderA.IsTrigger, CollisionMode.Entered));
            return true;
        }

        protected void OnSeperated(Fixture a, Fixture b)
        {
            var target = (a.Body.UserData as Entity);
            var hit = (b.Body.UserData as Entity);

            var colliderA = target.Get<Collider>();

            Engine.MessageHub.SendMessage(new CollisionMessage(target, hit, colliderA.IsTrigger, CollisionMode.Exited));
        }

        private Body CreateBody(Entity entity, Collider collider)
        {
            return BodyFactory.CreatePolygon(world, collider.Shape, collider.Density, entity);
        }
    }
}
