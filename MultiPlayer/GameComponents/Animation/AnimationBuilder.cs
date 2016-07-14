using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        private bool animatePhysics;

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

        public Animation CreateReverse()
        {
            var reversedTimes = new List<float>();
            times.ForEach(t => reversedTimes.Add(times[0] + times[times.Count - 1] - t));
            reversedTimes.Reverse();

            var reversedFrames = new List<KeyFrame>();
            reversedFrames.AddRange(frames);
            reversedFrames.Reverse();

            var animator = new Animation(reversedFrames, reversedTimes, seriesAccessors);
            animator.IsLooped = loops;
            animator.IsRelative = isRelative;
            animator.ResetOnComplete = resetsOnComplete;
            animator.Reverses = reverses;

            return animator;
        }

        public Animation Create()
        {
            var animator = new Animation(frames, times, seriesAccessors);
            animator.IsLooped = loops;
            animator.IsRelative = isRelative;
            animator.ResetOnComplete = resetsOnComplete;
            animator.Reverses = reverses;
            animator.AnimatePhysics = animatePhysics;

            return animator;
        }

        public AnimationBuilder AnimatePhysics()
        {
            this.animatePhysics = true;
            return this;
        }

        public AnimationBuilder ReflectHorizontal()
        {
            foreach (var frame in frames)
            {
                var value = (frame.Values[KeyFrame.POSITION_NAME] as Vector2?) ?? Vector2.Zero;

                frame.Values[KeyFrame.POSITION_NAME] = new Vector2(-value.X, value.Y);
            }

            return this;
        }

        public AnimationBuilder ReflectVertical()
        {
            foreach (var frame in frames)
            {
                var value = (frame.Values[KeyFrame.POSITION_NAME] as Vector2?) ?? Vector2.Zero;

                frame.Values[KeyFrame.POSITION_NAME] = new Vector2(value.X, -value.Y);
            }

            return this;
        }

        public AnimationBuilder ReflectRotation()
        {
            foreach (var frame in frames)
            {
                var value = (frame.Values[KeyFrame.ROTATION_NAME] as float?) ?? 0;

                frame.Values[KeyFrame.ROTATION_NAME] = -value;
            }

            return this;
        }
    }
}
