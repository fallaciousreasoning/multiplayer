using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;

namespace MultiPlayer.GameComponents.Animation
{
    public class PositionAccessor : IAccessor
    {
        public Entity Target { get; set; }

        public object Get()
        {
            return Target.Get<Transform>().Position;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (Vector2) value;
            var v2 = (Vector2) (relativeTo ?? Vector2.Zero);

            Target.Get<Transform>().Position = v2 + v1;
        }
    }
}
