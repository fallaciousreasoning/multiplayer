using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Animation.Components
{
    public class Animation
    {
        public bool Paused { get; set; }
        public bool ResetOnComplete { get; set; }

        public bool Repeats { get; set; }
        public bool Reverses { get; set; }

        public bool AnimatePhysics { get; set; }

        public List<KeyFrame> Frames { get; internal set; }
        public List<float> Times { get; internal set; }

        public float CurrentTime { get; set; }
    }
}
