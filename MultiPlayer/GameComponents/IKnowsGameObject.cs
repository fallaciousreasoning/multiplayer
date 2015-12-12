using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public interface IKnowsGameObject
    {
        GameObject GameObject { get; set; }
    }
}
