using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using Runner.Builders;
using Runner.Prefabs;

namespace Runner.Components
{
    public class Divable
    {
        public float MinSpeedForDive = 10;
        public Touching Touching;

        public LinkedList<Entity> Characters => Touching?.TouchingTag(Tags.CHARACTER);
    }
}
