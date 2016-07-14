using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class Vector2Calculator : ICalculator
    {
        public object Add(object first, object second)
        {
            return (Vector2)first + (Vector2)second;
        }

        public object Sub(object first, object second)
        {
            return (Vector2)first - (Vector2)second;
        }
    }
}
