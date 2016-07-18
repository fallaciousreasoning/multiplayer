using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core;

namespace MultiPlayer
{
    public class GameObjectFactory
    {
        private List<object> components;

        private Sprite renderer;
        private Transform transform;

        private string tag;

        public static GameObjectFactory New(bool independentTranslation=false, bool independentRotation=false, bool independentScale=false)
        {
            return new GameObjectFactory()
            {
                renderer = new Sprite(),
                transform = new Transform()
                {
                    IgnoreParentTranslation = independentTranslation,
                    IgnoreParentRotation = independentRotation,
                    IgnoreParentScale = independentScale
                },
                components = new List<object>()
            };
        }

        public GameObjectFactory With(params object[] c)
        {
            this.components.AddRange(c);
            return this;
        }

        public GameObjectFactory WithTexture(Texture2D texture)
        {
            renderer.Texture = texture;
            return this;
        }

        public GameObjectFactory WithTag(string tag)
        {
            this.tag = tag;
            return this;
        }

        public GameObjectFactory AtPosition(Vector2 position)
        {
            transform.LocalPosition = position;
            return this;
        }

        public GameObjectFactory AtScale(float scale)
        {
            return AtPosition(new Vector2(scale));
        }

        public GameObjectFactory AtScale(Vector2 scale)
        {
            transform.LocalScale = scale;
            return this;
        }

        public GameObjectFactory WithRotation(float rotation)
        {
            transform.LocalRotation = rotation;
            return this;
        }

        public GameObjectFactory With(Transform transform)
        {
            this.transform = transform;
            return this;
        }

        public GameObjectFactory With(Sprite sprite)
        {
            this.renderer = sprite;
            return this;
        }

        public GameObjectFactory WithChild(GameObject gameobject)
        {
            return With(gameobject);
        }

        public GameObjectFactory WithChild(string prefabName)
        {
            return WithChild(prefabName, Vector2.Zero);
        }

        public GameObjectFactory WithChild(string prefabName, Vector2 position)
        {
            return WithChild(prefabName, position, 0f, Vector2.One);
        }

        public GameObjectFactory WithChild(string prefabName, Vector2 position, float rotation, Vector2 scale)
        {
            var gameObject = Game1.Game.PrefabFactory.Build(prefabName, position, rotation, scale);
            return WithChild(gameObject);
        }

        public GameObject Create()
        {
            var gameObject = new GameObject(transform, renderer, components);
            gameObject.Tag.Name = tag;

            return gameObject;
        }
    }
}
