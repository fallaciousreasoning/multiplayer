using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class PrefabInfo
    {
        public string PrefabName { get; set; }

        public List<PrefabInfo> Children { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public float Rotation { get; set; }
    }
}
