using System.Collections.Generic;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.GameComponents.Animation
{
    public class AnimationController
    {
        private readonly Dictionary<string, Animation> animations;
        private Animation activeAnimation;
        private Entity target;

        private bool wasPlaying;

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

            Scene.ActiveScene.Engine.MessageHub.SendMessage(new AnimationStartedMessage());
            wasPlaying = true;
        }

        public void Pause()
        {
            activeAnimation?.Pause();
        }

        public void Stop()
        {
            activeAnimation?.Stop();

            BroadcastFinish();

            activeAnimation = null;
            wasPlaying = false;
        }

        public void Update(float step)
        {
            if (activeAnimation == null) return;

            activeAnimation.Update(step);

            if (wasPlaying && !activeAnimation.IsPlaying)
            {
                BroadcastFinish();
            }

            wasPlaying = activeAnimation.IsPlaying;
        }

        private void BroadcastFinish()
        {
            Scene.ActiveScene.Engine.MessageHub.SendMessage(new AnimationFinishedMessage());
        }

        public Entity Target
        {
            get { return target; }
            set
            {
                target = value;

                foreach (var animation in animations.Values)
                    animation.Target = target;
            }
        }

        public bool IsPlaying => activeAnimation != null && activeAnimation.IsPlaying;
    }
}
