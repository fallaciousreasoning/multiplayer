using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class RotationAccessor : IAccessor
    {
        public GameObject GameObject { get; set; }
        public object Get()
        {
            return GameObject.Transform.Rotation;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (float)value;
            var v2 = (float)(relativeTo ?? 0f);

            GameObject.Transform.Rotation = v2 + v1;
        }
    }
}
