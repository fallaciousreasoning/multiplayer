using System;

namespace MultiPlayer.Core.Animation
{
    public interface IAnimationFrame : ICloneable
    {
        object Interpolate(object start, object end, float percent);
        object FrameValue { get; }
    }
}
