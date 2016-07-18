using System.Collections.Generic;

namespace MultiPlayer.GameComponents.Animation
{
    public class AnimationController : IUpdateable, IKnowsGameObject
    {
        private readonly Dictionary<string, Animation> animations;
        private Animation activeAnimation;
        private GameObject gameObject;

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

            GameObject.HearsAnimationStarts.ForEach(s => s.OnAnimationStarted(animation));
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
            GameObject.HearsAnimationEnds.ForEach(e => e.OnAnimationEnd(activeAnimation));
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

        public bool IsPlaying => activeAnimation != null && activeAnimation.IsPlaying;
    }
}
