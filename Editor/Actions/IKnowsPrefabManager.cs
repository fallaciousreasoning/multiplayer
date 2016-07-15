using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;

namespace Editor.Actions
{
    internal interface IKnowsPrefabManager
    {
        PrefabFactory Prefabs { get; set; }
    }
}
