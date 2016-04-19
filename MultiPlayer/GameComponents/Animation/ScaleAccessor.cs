using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class ScaleAccessor : IAccessor
    {
        public GameObject GameObject { get; set; }
        public object Get()
        {
            return GameObject.Transform.Scale;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (Vector2)value;
            var v2 = (Vector2)relativeTo;

            GameObject.Transform.Scale = v2 * v1;
        }
    }
}
