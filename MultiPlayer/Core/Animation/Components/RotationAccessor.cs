using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;

namespace MultiPlayer.GameComponents.Animation
{
    public class RotationAccessor : IAccessor
    {
        public Entity Target { get; set; }
        public object Get()
        {
            return Target.Get<Transform>().Rotation;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (float)value;
            var v2 = (float)(relativeTo ?? 0f);

            Target.Get<Transform>().Rotation = v2 + v1;
        }
    }
}
