using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class Vector2Frame : BasicFrame<Vector2>
    {
        public static readonly Func<Vector2, Vector2, float, Vector2> Lerp =
            (start, end, percent) => Vector2.Lerp(start, end, percent);

        public static readonly Func<Vector2, Vector2, float, Vector2> Linear =
            (start, end, percent) => start + (end - start) * percent;

        public Vector2Frame(Vector2 value, Func<Vector2, Vector2, float, Vector2> interpolator=null)
            : base(value, interpolator??Linear)
        {
            
        }
    }
}
