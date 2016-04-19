using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Animation
{
    public interface IAnimationFrame : ICloneable
    {
        object Interpolate(object start, object end, float percent);
        object FrameValue { get; }
    }
}
