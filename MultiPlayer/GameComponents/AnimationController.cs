using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.Builders;

namespace MultiPlayer.GameComponents
{
    public class AnimationController : IUpdateable, IKnowsGameObject
    {
        private readonly Dictionary<string, Animation> animations;
        private Animation activeAnimation;
        private GameObject gameObject;

        public AnimationController(Dictionary<string, Animation> animations=null)
        {
            this.animations = animations ?? new Dictionary<string, Animation>();
        }

        public AnimationController Add(string animationName, Animation animation)
        {
            animations.Add(animationName, animation);
            return this;
        }

        public Animation Start(string animation)
        {
            var a = animations[animation];
            Start(a);
            return a;
        }

        public void Start(Animation animation)
        {
            activeAnimation = animation;
            animation.Start();
        }

        public void Pause()
        {
            activeAnimation?.Pause();
        }

        public void Stop()
        {
            activeAnimation?.Stop();
            activeAnimation = null;
        }

        public void Update(float step)
        {
            activeAnimation?.Update(step);
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set
            {
                gameObject = value;

                foreach (var animation in animations.Values)
                    animation.GameObject = gameObject;
            }
        }
    }
}
