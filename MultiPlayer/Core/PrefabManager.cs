using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;
using MultiPlayer.Extensions;
using MultiPlayer.Factories;

namespace MultiPlayer.Core
{
    public class PrefabManager
    {
        private readonly Dictionary<string, Func<EntityBuilder>> entityBuilders = new Dictionary<string, Func<EntityBuilder>>();
        private readonly bool ignoreCase;

        private readonly Engine manager;

        public PrefabManager(Engine manager, bool ignoreCase = true)
        {
            this.manager = manager;
            this.ignoreCase = ignoreCase;
        }

        public void RegisterPrefab(string name, Func<EntityBuilder> builder)
        {
            name = ignoreCase ? name.ToLower() : name;
            if (entityBuilders.ContainsKey(name)) throw new Exception($"You've already created a prefab called {name}");

            entityBuilders.Add(name, builder);
        }

        public List<Entity> Instantiate(string name)
        {
            return Instantiate(name, Vector2.Zero);
        }

        public List<Entity> Instantiate(string name, Vector2 position)
        {
            return Instantiate(name, position, 0);
        }

        public List<Entity> Instantiate(string name, Vector2 position, float rotation)
        {
            return Instantiate(name, position, rotation, Vector2.One);
        }

        public List<Entity> Instantiate(string name, Vector2 position, float rotation, Vector2 scale)
        {
            var entities = Build(name, position, rotation, scale);

            entities.Reverse();
            EnumerableExtensions.ForEach(entities, manager.AddEntity);

            return entities;
        }

        public Entity Instantiate(Entity entity)
        {
            manager.AddEntity(entity);
            return entity;
        }

        public List<Entity> Build(string name)
        {
            return Build(name, Vector2.Zero);
        }

        public List<Entity> Build(string name, Vector2 position)
        {
            return Build(name, position, 0);
        }

        public List<Entity> Build(string name, Vector2 position, float rotation)
        {
            return Build(name, position, rotation, Vector2.One);
        }

        public List<Entity> Build(string name, Vector2 position, float rotation, Vector2 scale)
        {
            name = ignoreCase ? name.ToLower() : name;
            if (!entityBuilders.ContainsKey(name)) throw new Exception($"You don't have a prefab called {name}");

            var entities = entityBuilders[name]()
                .AtPosition(position)
                .AtRotation(rotation)
                .AtScale(scale)
                .Create();

            return entities;
        }
    }
}
