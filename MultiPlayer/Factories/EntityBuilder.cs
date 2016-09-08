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
        private Dictionary<Type, object> Components => entities[entities.Count - 1];
        private readonly List<Dictionary<Type, object>> entities = new List<Dictionary<Type, object>>();

        public EntityBuilder NewEntity()
        {
            entities.Add(new Dictionary<Type, object>());
            return this;
        }

        public EntityBuilder WithTag(string tag)
        {
            if (Components.ContainsKey(typeof(Tag))) Components.Add(typeof(Tag), new Tag());

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

        public static EntityBuilder New()
        {
            return new EntityBuilder().With<Transform>();
        }

        public IEnumerable<Entity> Create()
        {
            var created = new List<Entity>();

            foreach (var components in entities)
            {
                var entity = new Entity();
                foreach (var key in components.Keys)
                    entity.Add(components[key]);

                created.Add(entity);
            }

            return created;
        }

        public Entity CreateLast()
        {
            var entity = new Entity();
            foreach (var key in Components.Keys)
                entity.Add(Components[key]);

            return entity;
        }

        public EntityBuilder AtPosition(Vector2 position)
        {
            Get<Transform>().Position = position;
            return this;
        }
    }
}
