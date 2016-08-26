using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Animation
{
    public class FloatCalculator : ICalculator
    {
        public object Add(object first, object second)
        {
            return (float)first + (float)second;
        }

        public object Sub(object first, object second)
        {
            return (float)first - (float)second;
        }
    }
}
