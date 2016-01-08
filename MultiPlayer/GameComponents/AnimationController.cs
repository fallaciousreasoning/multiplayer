using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Runner.Builders
{
    public class AnimationController : IUpdateable, IKnowsGameObject
    {
        public bool IsRelative { get; set; } = true;
        public bool ResetOnComplete { get; set; }
        public bool Reverses { get; set; }
        public bool IsLooped { get; set; }
        public bool IsPlaying { get { return playing; } }

        public Transform AnimationTarget { get { return GameObject.Transform; } }

        private readonly List<float> times = new List<float>(); 
        private readonly List<KeyFrame> frames = new List<KeyFrame>();

        private float currentTime;

        private int currentFrame = 0;
        private bool playing;
        private bool paused;

        private float lastTime;
        private KeyFrame lastFrame;
        private KeyFrame firstFrame;

        private bool reversing;

        internal AnimationController(List<KeyFrame> frames, List<float> times)
        {
            this.frames = frames;
            this.times = times;
        }

        public void Stop()
        {
            Complete();
        }

        public void Resume()
        {
            paused = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Start()
        {
            Console.WriteLine("Starting animation!");

            playing = true;
            reversing = false;
            currentFrame = 0;
            currentTime = 0;
            lastTime = 0;

            firstFrame = new KeyFrame(AnimationTarget);

            //If our first frame is a setup frame, set our transform to that position
            if (times.Count > 0 && times[0] == 0)
            {
                lastFrame = frames[currentFrame++];
            }
            //Otherwise, animate it from where it is
            else
            {
                lastFrame = firstFrame;
            }
        }

        public void Update(float step)
        {
            if (!playing || paused) return;

            if (!reversing)
                currentTime += step;
            else currentTime -= step;

            if (ShouldMoveNextFrame())
            {
                Animate(true);
                NextFrame();
            } else Animate();
        }

        private bool ShouldMoveNextFrame()
        {
            return reversing ?  times[currentFrame] > currentTime : times[currentFrame] <= currentTime;
        }

        private void NextFrame()
        {
            lastFrame = frames[currentFrame];
            lastTime = times[currentFrame];

            currentFrame += reversing ? -1 : 1;
            Console.WriteLine($"Moving to frame {currentFrame}");

            if (currentFrame >= frames.Count || currentFrame < 0)
            {
                Complete();
            }
        }

        private void Animate(bool finish = false)
        {
            Animate(frames[currentFrame], finish);
        }

        private void Animate(KeyFrame frame, bool finish = false)
        {
            var time = times[currentFrame];

            var percentage = finish ? 1 : (currentTime - lastTime)/(time - lastTime);
            var s = frame.SmoothingFunction;

            var position = new Vector2(s(lastFrame.Position.X, frame.Position.X, percentage), s(lastFrame.Position.Y, frame.Position.Y, percentage));
            
            var rotation = s(lastFrame.Rotation, frame.Rotation, percentage);
            
            var scale = new Vector2(s(lastFrame.Scale.X, frame.Scale.X, percentage), s(lastFrame.Scale.Y, frame.Scale.Y, percentage));

            if (IsRelative)
            {
                position += firstFrame.Position;
                rotation += firstFrame.Rotation;
                scale *= firstFrame.Scale;
            }

            AnimationTarget.LocalPosition = position;
            AnimationTarget.LocalRotation = rotation;
            AnimationTarget.LocalScale = scale;
        }

        private void Complete()
        {
            Console.WriteLine("Completing");
            //Make sure we're on the last frame
            currentFrame = reversing ? 0 : frames.Count - 1;
            Animate(true);

            if (Reverses && !reversing)
            {
                reversing = true;
                return;
            }

            //If we're resetting, focus on the first frame
            if (ResetOnComplete)
                Animate(firstFrame, true);

            playing = false;
            if (IsLooped) Start();
        }

        public GameObject GameObject { get; set; }
    }

    public struct KeyFrame
    {
        public static Func<float, float, float, float> Lerp
            => MathHelper.Lerp;

        public static Func<float, float, float, float> Linear => (start, end, percent) => start + (end - start)*percent;
         
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Func<float, float, float, float> SmoothingFunction;

        public KeyFrame(Transform transform)
            : this(transform.LocalPosition, transform.LocalRotation, transform.LocalScale)
        {
        }

        public KeyFrame(Vector2 position, float rotation) : this(position, rotation, Vector2.One)
        {
        }

        public KeyFrame(Vector2 position) : this(position, 0, Vector2.One)
        {
        }

        public KeyFrame(float rotation) : this(Vector2.Zero, rotation, Vector2.One) { }

        public KeyFrame(Vector2 position, float rotation, Vector2 scale) : this(position, rotation, scale, Linear)
        {
            
        }

        public KeyFrame(Vector2 position, float rotation, Vector2 scale, Func<float, float, float, float> smoothingFunction)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;

            SmoothingFunction = smoothingFunction;
        }
    }

    public class AnimationBuilder
    {
        private readonly List<KeyFrame> frames = new List<KeyFrame>();
        private readonly List<float> times = new List<float>();
        private bool loops;
        private bool isRelative;
        private bool reverses;
        private bool resetsOnComplete;

        private AnimationBuilder()
        {
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

        public AnimationController Create()
        {
            var animator = new AnimationController(frames, times);
            animator.IsLooped = loops;
            animator.IsRelative = isRelative;
            animator.ResetOnComplete = resetsOnComplete;
            animator.Reverses = reverses;

            return animator;
        }
    }
}
