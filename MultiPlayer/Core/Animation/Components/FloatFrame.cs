using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class FloatFrame : BasicFrame<float>
    {
        public static readonly Func<float, float, float, float> Lerp =
            (start, end, percent) => MathHelper.Lerp(start, end, percent);

        public static readonly Func<float, float, float, float> Linear =
            (start, end, percent) => start + (end - start) * percent;

        public FloatFrame(float value, Func<float, float, float, float> interpolator = null)
            : base(value, interpolator??Linear)
        {

        }
    }
}
