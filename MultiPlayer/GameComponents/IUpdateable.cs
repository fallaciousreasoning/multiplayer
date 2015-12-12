using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public interface IUpdateable
    {
        void Update(float step);
    }
}
