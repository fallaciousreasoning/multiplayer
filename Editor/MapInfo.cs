using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.GameComponents;

namespace Editor
{
    public class MapInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public PrefabInfo Scene { get; set; }
    }
}
