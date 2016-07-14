using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer.GameComponents;

namespace MultiPlayer.Core
{
    public class PrefabFactory
    {
        private readonly Dictionary<string, Func<GameObject>> gameObjectBuilders = new Dictionary<string, Func<GameObject>>();
        private readonly bool ignoreCase;

        private ComponentManager<GameObject> manager; 
        
        public PrefabFactory(ComponentManager<GameObject> manager, bool ignoreCase = true)
        {
            this.manager = manager;
            this.ignoreCase = ignoreCase;

            RegisterPrefab("empty", () => GameObjectFactory.New().Create());
        }

        public void RegisterPrefab(string name, Func<GameObject> builder)
        {
            name = ignoreCase ? name.ToLower() : name;
            if (gameObjectBuilders.ContainsKey(name)) throw new Exception($"You've already created a prefab called {name}");
            
            gameObjectBuilders.Add(name, builder);
        }

        public GameObject Instantiate(PrefabInfo info)
        {
            return Instantiate(info, null);
        }

        public GameObject Instantiate(PrefabInfo info, GameObject parent)
        {
            var gameObject = Instantiate(info.PrefabName, info.Position, info.Rotation, info.Scale, parent);
            info.Children?.ForEach(child => Instantiate(child, gameObject));
            return gameObject;
        }

        public GameObject Instantiate(string name)
        {
            return Instantiate(name, Vector2.Zero);
        }

        public GameObject Instantiate(string name, Vector2 position)
        {
            return Instantiate(name, position, 0f);
        }

        public GameObject Instantiate(string name, Vector2 position, float rotation)
        {
            return Instantiate(name, position, rotation, Vector2.One);
        }

        public GameObject Instantiate(string name, Vector2 position, float rotation, Vector2 scale)
        {
            return Instantiate(name, position, rotation, scale, null);
        }

        public GameObject Instantiate(string name, GameObject parent)
        {
            return Instantiate(name, Vector2.Zero, parent);
        }

        public GameObject Instantiate(string name, Vector2 position, GameObject parent)
        {
            return Instantiate(name, position, 0f, parent);
        }

        public GameObject Instantiate(string name, Vector2 position, float rotation, GameObject parent)
        {
            return Instantiate(name, position, rotation, Vector2.One, parent);
        }

        public GameObject Instantiate(string name, Vector2 position, float rotation, Vector2 scale, GameObject parent)
        {
            var gameObject = Build(name, position, rotation, scale);
            if (parent != null)
                parent.DelayedAdd(gameObject);
            else
                manager.DelayedAdd(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(GameObject gameObject)
        {
            manager.DelayedAdd(gameObject);
            return gameObject;
        }

        public GameObject Build(string name)
        {
            return Build(name, Vector2.Zero);
        }

        public GameObject Build(string name, Vector2 position)
        {
            return Build(name, position, 0f);
        }

        public GameObject Build(string name, Vector2 position, float rotation)
        {
            return Build(name, position, rotation, Vector2.One);
        }

        public GameObject Build(string name, Vector2 position, float rotation, Vector2 scale)
        {
            name = ignoreCase ? name.ToLower() : name;
            if (!gameObjectBuilders.ContainsKey(name)) throw new Exception($"You don't have a prefab called {name}");

            var gameObject = gameObjectBuilders[name]();
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = scale;

            return gameObject;
        }
    }
}
