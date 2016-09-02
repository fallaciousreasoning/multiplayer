using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Animation
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
