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

        private List<EntityBuilder> childCreators = new List<EntityBuilder>();

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

        public Entity Create()
        {
            var root = CreateRoot();
            var children = root.HasComponent<HasChildren>() ? root.Get<HasChildren>() : null;

            foreach (var builder in childCreators)
            {
                var entity = builder.Create();
                children.AddChild(entity);
            }

            return root;
        }

        private Entity CreateRoot()
        {
            var entity = new Entity();

            foreach (var key in Components.Keys)
                entity.Add(Components[key]);

            if (childCreators.Count > 0 && !entity.HasComponent<HasChildren>())
                entity.Add<HasChildren>();

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
            childCreators.Add(child);
            return this;
        }
    }
}
