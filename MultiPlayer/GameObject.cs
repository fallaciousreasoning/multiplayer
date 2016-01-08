using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Physics;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace MultiPlayer
{
    public class GameObject : ComponentManager<object>, IHearsDestroy, IDestroyable
    {
        public VelocityController Velocity { get; private set; }
        public Transform Transform { get; private set; }
        public Sprite Renderer { get; private set; }

        public readonly List<IHearsCollision> HearCollisions = new List<IHearsCollision>();
        public readonly List<IHearsTrigger> HearTriggers = new List<IHearsTrigger>(); 

        private readonly List<IHearsDestroy> hearDestroy = new List<IHearsDestroy>();
        
        internal GameObject(Transform transform, Sprite sprite, List<object> components)
        {
            this.Transform = transform;
            this.Renderer = sprite;

            Add(Transform);
            Add(sprite);

            components.ForEach(DelayedAdd);
        }

        protected override sealed void Add(object component)
        {
            base.Add(component);

            if (component is VelocityController)
                this.Velocity = (component as VelocityController);

            if (component is IKnowsGameObject)
                (component as IKnowsGameObject).GameObject = this;

            if (component is IHearsDestroy)
                hearDestroy.Add(component as IHearsDestroy);
        }

        /// <summary>
        /// Called when the gameobject is destoryed
        /// </summary>
        public void OnDestroy()
        {
            hearDestroy.ForEach(c => c.OnDestroy());
        }

        /// <summary>
        /// Indicates whether this object should be removed
        /// </summary>
        public bool ShouldRemove { get; set; }
    }
}
