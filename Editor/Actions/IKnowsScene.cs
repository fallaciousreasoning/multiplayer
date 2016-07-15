using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;

namespace Editor.Actions
{
    internal interface IKnowsScene
    {
        GameObject Scene { get; set; }
    }
}
