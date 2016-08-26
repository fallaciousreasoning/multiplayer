using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Animation
{
    public interface ICalculator
    {
        object Add(object first, object second);
        object Sub(object first, object second);
    }
}
