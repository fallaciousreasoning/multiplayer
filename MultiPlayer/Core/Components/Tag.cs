using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Components
{
    public class Tag
    {
        public HashSet<string> Tags = new HashSet<string>();

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }
    }
}
