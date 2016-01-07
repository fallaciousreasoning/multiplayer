using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer
{
    public static class RandomUtil
    {
        private static Random random = new Random();

        public static float RandomFloat(float min, float max)
        {
            return (float) (random.NextDouble()*Math.Abs(max - min) - min);
        }

        public static float RandomFloat(float max)
        {
            return RandomFloat(0, max);
        }

        public static float RandomFloat()
        {
            return RandomFloat(1);
        }

        public static Vector2 RandomVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(RandomFloat(min.X, max.X), RandomFloat(min.Y, max.Y));
        }
    }
}
