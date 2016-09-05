using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Animation
{
    public class AnimationContainer
    {
        public Dictionary<string, Animation> Animation { get; private set; } = new Dictionary<string, Animation>();
        public Animation CurrentAnimation { get; set; }
        public bool WasPlaying { get; set; }

        public AnimationContainer Add(string name, Animation animation)
        {
            Animation.Add(name, animation);
            return this;
        }
    }
}
