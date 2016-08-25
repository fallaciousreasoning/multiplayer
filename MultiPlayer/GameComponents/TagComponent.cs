using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class TagComponent
    {
        public HashSet<string> Tags { get; set; } = new HashSet<string>();

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
