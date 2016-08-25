using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Animation;
using MultiPlayer.GameComponents.Physics;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace MultiPlayer
{
    public class GameObject : ComponentManager<object>, IHearsDestroy, IDestroyable
    {
        public string Name { get; set; } = "GameObject";

        public VelocityController Velocity { get; private set; }
        public Transform Transform { get; private set; }
        public TagComponent Tag { get; private set; }
        public Sprite Renderer { get; private set; }

        public readonly List<IHearsCollision> HearCollisions = new List<IHearsCollision>();
        public readonly List<IHearsTrigger> HearTriggers = new List<IHearsTrigger>(); 

        public readonly List<IHearsAnimationStart> HearsAnimationStarts = new List<IHearsAnimationStart>();
        public readonly List<IHearsAnimationEnd> HearsAnimationEnds = new List<IHearsAnimationEnd>();

        private readonly List<IHearsDestroy> hearDestroy = new List<IHearsDestroy>();
        private readonly List<IHearsAdd> hearAdd = new List<IHearsAdd>();

        public GameObject(Transform transform, Sprite sprite, List<object> components)
        {
            this.Transform = transform;
            this.Renderer = sprite;
            this.Tag = new TagComponent();

            Add(Transform);
            Add(sprite);

            components.ForEach(DelayedAdd);
        }

        protected sealed override void Add(object component)
        {
            base.Add(component);

            if (component is VelocityController)
                this.Velocity = (component as VelocityController);

            if (component is IKnowsGameObject)
                (component as IKnowsGameObject).GameObject = this;

            if (component is IHearsDestroy)
                hearDestroy.Add(component as IHearsDestroy);

            if (component is IHearsTrigger)
                HearTriggers.Add(component as IHearsTrigger);

            if (component is IHearsCollision)
                HearCollisions.Add(component as IHearsCollision);

            if (component is IHearsAnimationStart)
                HearsAnimationStarts.Add(component as IHearsAnimationStart);

            if (component is IHearsAnimationEnd)
                HearsAnimationEnds.Add(component as IHearsAnimationEnd);

            hearAdd.ForEach(h => h.OnAdded(component));

            if (component is IHearsAdd)
                hearAdd.Add(component as IHearsAdd);
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
