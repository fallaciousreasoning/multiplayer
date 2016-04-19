using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class Animation : IUpdateable, IKnowsGameObject
    {
        public bool IsRelative { get; set; } = true;
        public bool ResetOnComplete { get; set; }
        public bool Reverses { get; set; }
        public bool IsLooped { get; set; }
        public bool IsPlaying { get { return playing; } }

        public Transform AnimationTarget { get { return GameObject.Transform; } }

        private readonly Dictionary<string, IAccessor> seriesAccessors;
        private readonly List<string> series;
         
        private readonly List<float> times; 
        private readonly List<KeyFrame> frames;

        private float currentTime;

        private int currentFrame = 0;
        private bool playing;
        private bool paused;

        private float lastTime;

        private Dictionary<string, object> lastFrame;
        private Dictionary<string, object> firstFrame;
        private Dictionary<string, object> Frame { get { return frames[currentFrame].Values; } } 

        private bool reversing;
        private GameObject gameObject;

        internal Animation(List<KeyFrame> frames, List<float> times, Dictionary<string, IAccessor> seriesAccessors)
        {
            this.frames = frames;
            this.times = times;
            this.seriesAccessors = seriesAccessors;

            //Add all the series from our frames
            var uniqueSeries = new HashSet<string>();
            frames.ForEach(f => f.Series.ForEach(s => uniqueSeries.Add(s)));
            series = uniqueSeries.ToList();

            if (frames.Any(f => !f.ContainsAll(series)))
            {
                throw new Exception("Every keyframe must specify a value for all series");
            }
        }

        private void Reset()
        {
            foreach (var name in series)
            {
                var accessor = seriesAccessors[name];
                var value = firstFrame[name];

                accessor.Set(value, null);
            }
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

        private Dictionary<string, object> ExtractAllSeries()
        {
            var values = new Dictionary<string, object>();

            foreach (var key in series)
            {
                var accessor = seriesAccessors[key];
                var value = accessor.Get();
                values.Add(key, value);
            }

            return values;
        }

        public void Start()
        {
            Console.WriteLine("Starting animation!");

            playing = true;
            reversing = false;
            currentFrame = 0;
            currentTime = 0;
            lastTime = 0;

            firstFrame = ExtractAllSeries();

            //If our first frame is a setup frame, set our transform to that position
            if (times.Count > 0 && times[0] == 0)
            {
                lastFrame = frames[currentFrame++].Values;
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
            lastFrame = frames[currentFrame].Values;
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
            foreach (var seriesName in series)
            {
                var seriesFrame = frame.GetFrame(seriesName);
                var accessor = seriesAccessors[seriesName];
                var value = seriesFrame.Interpolate(lastFrame[seriesName], Frame[seriesName], percentage);

                accessor.Set(value, IsRelative ? firstFrame[seriesName] : null);
            }
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
                Reset();

            playing = false;
            if (IsLooped) Start();
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set
            {
                gameObject = value;
                foreach (var s in series)
                {
                    seriesAccessors[s].GameObject = value;
                }
            }
        }
    }
}
