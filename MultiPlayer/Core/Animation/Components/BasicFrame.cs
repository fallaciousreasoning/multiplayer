using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class BasicFrame<T> : IAnimationFrame
    {
        private readonly Func<T, T, float, T> interpolater; 

        public object FrameValue { get; protected set; }

        public BasicFrame(T frameValue, Func<T, T, float, T> interpolater)
        {
            this.FrameValue = frameValue;
            this.interpolater = interpolater;
        }

        public object Interpolate(object start, object end, float percent)
        {
            return interpolater((T)start, (T)end, percent);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
