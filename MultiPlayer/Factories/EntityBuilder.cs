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
        private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();

        public EntityBuilder WithTag(string tag)
        {
            throw new NotImplementedException();
        }

        public EntityBuilder WithTexture(Texture2D texture)
        {
            return With(new Sprite() {Texture = texture});
        }

        public EntityBuilder With(object component)
        {
            components.Add(component.GetType(), component);
            return this;
        }

        public EntityBuilder With<T>() where T : class, new()
        {
            return With(new T());
        }

        public object Get(Type type)
        {
            return components[type];
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public static EntityBuilder New()
        {
            return new EntityBuilder().With<Transform>();
        }

        public Entity Create()
        {
            var entity = new Entity();
            foreach (var key in components.Keys)
                entity.Add(components[key]);

            return entity;
        }

        public EntityBuilder AtPosition(Vector2 position)
        {
            Get<Transform>().Position = position;
            return this;
        }
    }
}
