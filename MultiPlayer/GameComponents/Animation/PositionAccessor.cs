using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class PositionAccessor : IAccessor
    {
        public GameObject GameObject { get; set; }

        public object Get()
        {
            return GameObject.Transform.Position;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (Vector2) value;
            var v2 = (Vector2) relativeTo;

            GameObject.Transform.Position = v2 + v1;
        }
    }
}
