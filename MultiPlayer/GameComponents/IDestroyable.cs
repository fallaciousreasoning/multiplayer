using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer.Annotations;

namespace MultiPlayer.GameComponents
{
    public interface IDestroyable
    {
        [EditorIgnore]
        bool ShouldRemove { get; set; }
    }
}
