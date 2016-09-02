using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Animation.Components;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Animation.Builders
{
    public class AnimationBuilder
    {
        private ISet<string> series;

        private List<float> times = new List<float>();
        private List<KeyFrameBuilder> frameBuilders = new List<KeyFrameBuilder>();

        private bool repeats;
        private bool animatePhysics;
        private bool reverses;
        private bool resetOnComplete;

        private AnimationBuilder()
        {
            
        }

        public AnimationBuilder Repeats(bool repeats = true)
        {
            this.repeats = repeats;
            return this;
        }

        public AnimationBuilder AnimatePhysics(bool animatePhysics = true)
        {
            this.animatePhysics = animatePhysics;
            return this;
        }

        public AnimationBuilder Reverses(bool reverses = true)
        {
            this.reverses = reverses;
            return this;
        }

        public AnimationBuilder Resets(bool resets = true)
        {
            this.resetOnComplete = resets;
            return this;
        }

        public AnimationBuilder WithFrameBuilder(float time, KeyFrameBuilder builder)
        {
            if (!builder.HasSeries(series)) throw new ArgumentException("Builder doesn't have all the series we need!");

            times.Add(time);
            frameBuilders.Add(builder);

            return this;
        }

        public Components.Animation Create()
        {
            return new Components.Animation()
            {
                Reverses = reverses,
                Repeats = repeats,
                AnimatePhysics = animatePhysics,
                ResetOnComplete = resetOnComplete,
                Frames = (from frame in frameBuilders select frame.Build()).ToList(),
                Times = (from time in times select time).ToList()
            };
        }

        public AnimationBuilder New(IEnumerable<string> series)
        {
            return new AnimationBuilder() {series = series.ToSet()};
        }
    }
}
