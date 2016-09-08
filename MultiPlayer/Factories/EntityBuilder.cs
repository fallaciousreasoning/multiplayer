using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Factories
{
    public class EntityBuilder
    {
        private Dictionary<Type, object> Components = new Dictionary<Type, object>();
        private List<EntityBuilder> children = new List<EntityBuilder>();

        public EntityBuilder WithTag(string tag)
        {
            if (!Components.ContainsKey(typeof(Tag))) Components.Add(typeof(Tag), new Tag());

            var tagComponent = Components[typeof(Tag)] as Tag;
            tagComponent.AddTag(tag);

            return this;
        }

        public EntityBuilder WithTexture(Texture2D texture)
        {
            return With(new Sprite() {Texture = texture});
        }

        public EntityBuilder With(object component)
        {
            Components.Add(component.GetType(), component);
            return this;
        }

        public EntityBuilder With<T>() where T : class, new()
        {
            return With(new T());
        }

        public object Get(Type type)
        {
            return Components[type];
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public static EntityBuilder New(bool positionIndependent=false, bool rotationIndependent=false, bool scaleIndependent=false)
        {
            return new EntityBuilder().With(new Transform()
            {
                PositionIndependent = positionIndependent,
                RotationIndependent = rotationIndependent,
                ScaleIndependent = scaleIndependent
            });
        }

        public List<Entity> Create(Entity parent=null)
        {
            var created = new List<Entity>();

            var root = CreateRoot(parent);
            created.Add(root);

            foreach (var builder in children)
            {
                var entities = builder.Create(root);
                created.AddRange(entities);
            }

            return created;
        }

        public Entity CreateRoot(Entity parent=null)
        {
            var entity = new Entity();
            foreach (var key in Components.Keys)
                entity.Add(Components[key]);

            if (parent != null && entity.HasComponent<Transform>() && parent.HasComponent<Transform>())
                entity.Get<Transform>().Parent = parent.Get<Transform>();

            return entity;
        }

        public EntityBuilder AtPosition(Vector2 position)
        {
            Get<Transform>().Position = position;
            return this;
        }

        public EntityBuilder AtRotation(float rotation)
        {
            Get<Transform>().Rotation = rotation;
            return this;
        }

        public EntityBuilder AtScale(Vector2 scale)
        {
            Get<Transform>().Scale = scale;
            return this;
        }

        public EntityBuilder WithChild(EntityBuilder child)
        {
            children.Add(child);
            return this;
        }
    }
}
