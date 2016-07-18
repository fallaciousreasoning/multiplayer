using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;

namespace Editor.Scripts.Placer
{
    public class PlacerSettings
    {
        public float MinSize { get; set; } = 0.5f;
        public float GridSize { get; set; } = 0.25f;
        public bool SnapToGrid { get; set; } = true;

        public bool NoSize { get; set; }

        public string PlacingPrefab { get; set; }

        public GameObject Shadow { get; set; }
    }
}
