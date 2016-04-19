using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Animation
{
    public class AnimationBuilder
    {
        private readonly Dictionary<string, IAccessor> seriesAccessors = new Dictionary<string, IAccessor>(); 
        private readonly List<KeyFrame> frames = new List<KeyFrame>();
        private readonly List<float> times = new List<float>();
        private bool loops;
        private bool isRelative;
        private bool reverses;
        private bool resetsOnComplete;

        private AnimationBuilder()
        {
            seriesAccessors.Add(KeyFrame.POSITION_NAME, new PositionAccessor());
            seriesAccessors.Add(KeyFrame.SCALE_NAME, new ScaleAccessor());
            seriesAccessors.Add(KeyFrame.ROTATION_NAME, new RotationAccessor());
        }

        public static AnimationBuilder New()
        {
            return new AnimationBuilder();
        }

        public AnimationBuilder InsertFrame(float timeStamp, KeyFrame frame)
        {
            var index = 0;
            while (index < frames.Count && times[index] < timeStamp)
                index++;

            frames.Insert(index, frame);
            times.Insert(index, timeStamp);

            return this;
        }

        public AnimationBuilder Loops(bool loops)
        {
            this.loops = loops;
            return this;
        }

        public AnimationBuilder IsRelative(bool isRelative)
        {
            this.isRelative = isRelative;
            return this;
        }

        public AnimationBuilder Reverses(bool reverses)
        {
            this.reverses = reverses;
            return this;
        }

        public AnimationBuilder ResetsOnComplete(bool resetsOnComplete)
        {
            this.resetsOnComplete = resetsOnComplete;
            return this;
        }

        public Animation Create()
        {
            var animator = new Animation(frames, times, seriesAccessors);
            animator.IsLooped = loops;
            animator.IsRelative = isRelative;
            animator.ResetOnComplete = resetsOnComplete;
            animator.Reverses = reverses;

            return animator;
        }
    }
}
