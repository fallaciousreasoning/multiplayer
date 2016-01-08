using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Runner.Builders
{
    public class AnimationController : IUpdateable
    {
        public bool IsRelative = true;
        public bool ResetOnComplete;

        public List<float> Times = new List<float>(); 
        public List<KeyFrame> Frames = new List<KeyFrame>();

        public float CurrentTime;

        public int CurrentFrame = 0;
        public bool Playing;
        public bool Paused;

        public bool IsLooped;

        public Transform Animating;

        private float lastTime;
        private KeyFrame lastFrame;
        private KeyFrame firstFrame;

        public AnimationController(Transform animating)
        {
            Animating = animating;
        }

        public void Stop()
        {
            Complete();
        }

        public void Resume()
        {
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Start()
        {
            Playing = true;
            CurrentFrame = 0;
            CurrentTime = 0;
            lastTime = 0;

            firstFrame = new KeyFrame(Animating);

            //If our first frame is a setup frame, set our transform to that position
            if (Times.Count > 0 && Times[0] == 0)
            {
                lastFrame = Frames[CurrentFrame++];
            }
            //Otherwise, animate it from where it is
            else
            {
                lastFrame = firstFrame;
            }
        }

        public void Update(float step)
        {
            if (!Playing || Paused) return;

            CurrentTime += step;

            if (ShouldMoveNextFrame())
            {
                Animate(true);
                NextFrame();
            } else Animate();
        }

        private bool ShouldMoveNextFrame()
        {
            return Times[CurrentFrame] <= CurrentFrame;
        }

        private void NextFrame()
        {
            lastFrame = Frames[CurrentFrame];
            lastTime = Times[CurrentFrame];

            CurrentFrame++;
            if (CurrentFrame >= Frames.Count)
            {
                Complete();
            }
        }

        private void Animate(bool finish = false)
        {
            Animate(Frames[CurrentFrame], finish);
        }

        private void Animate(KeyFrame frame, bool finish = false)
        {
            var time = Times[CurrentFrame];

            var percentage = finish ? 1 : (CurrentTime - lastTime)/(time - lastTime);
            var s = frame.SmoothingFunction;

            var position = new Vector2(s(lastFrame.Position.X, frame.Position.X, percentage), s(lastFrame.Position.Y, frame.Position.Y, percentage));
            var rotation = s(lastFrame.Rotation, frame.Rotation, percentage);
            var scale = new Vector2(s(lastFrame.Scale.X, frame.Scale.X, percentage), s(lastFrame.Scale.Y, frame.Scale.Y, percentage));

            Animating.LocalPosition = position;
            Animating.LocalRotation = rotation;
            Animating.LocalScale = scale;
        }

        private void Complete()
        {
            //Make sure we're on the last frame
            CurrentFrame = Frames.Count - 1;
            Animate(true);

            //If we're resetting, focus on the first frame
            if (ResetOnComplete)
                Animate(firstFrame, true);

            Playing = false;
            if (IsLooped) Start();
        }
    }

    public struct KeyFrame
    {
        public static Func<float, float, float, float> Lerp
            => MathHelper.Lerp;

        public static Func<float, float, float, float> Linear => (start, end, percent) => end + (end - start)*percent;
         
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Func<float, float, float, float> SmoothingFunction;

        public KeyFrame(Transform transform)
            : this(transform.LocalPosition, transform.LocalScale, transform.LocalRotation)
        {
        }

        public KeyFrame(Vector2 position, Vector2 scale, float rotation) : this(position, scale, rotation, Linear)
        {
            
        }

        public KeyFrame(Vector2 position, Vector2 scale, float rotation, Func<float, float, float, float> smoothingFunction)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;

            SmoothingFunction = smoothingFunction;
        }
    }
}
