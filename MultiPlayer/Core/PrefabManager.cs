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

        public IEnumerable<Entity> Instantiate(string name)
        {
            var entities = Build(name);

            entities.Foreach(manager.AddEntity);

            return entities;
        }

        public Entity Instantiate(Entity entity)
        {
            manager.AddEntity(entity);
            return entity;
        }

        public IEnumerable<Entity> Build(string name)
        {
            name = ignoreCase ? name.ToLower() : name;
            if (!entityBuilders.ContainsKey(name)) throw new Exception($"You don't have a prefab called {name}");

            var entities = entityBuilders[name]().Create();
            return entities;
        }
    }
}
