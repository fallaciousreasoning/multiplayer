using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;

namespace Runner.Components
{
    public class Touching
    {
        public Dictionary<string, int> TouchingCounts = new Dictionary<string, int>();

        public void Touched(IEnumerable<string> tags)
        {
            tags.Foreach(Touched);
        }
         
        public void Touched(string tag)
        {
            if (!TouchingCounts.ContainsKey(tag)) TouchingCounts.Add(tag, 0);

            TouchingCounts[tag]++;
        }

        public void Seperated(IEnumerable<string> tags)
        {
            tags.Foreach(Seperated);
        }

        public void Seperated(string tag)
        {
            TouchingCounts[tag]--;
        }

        public bool IsTouching(string tag)
        {
            return TouchingCounts.ContainsKey(tag) && TouchingCounts[tag] > 0;
        }
    }
}
