using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer.Annotations;

namespace MultiPlayer.GameComponents
{
    public interface IKnowsGameObject
    {
        [EditorIgnore]
        GameObject GameObject { get; set; }
    }
}
