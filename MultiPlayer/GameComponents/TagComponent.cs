using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class TagComponent
    {
        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }
    }
}
