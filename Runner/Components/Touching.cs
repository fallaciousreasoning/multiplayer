using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Extensions;

namespace Runner.Components
{
    public class Touching
    {
        public Dictionary<string, int> TouchingCounts = new Dictionary<string, int>();
        public Dictionary<string, LinkedList<Entity>> TouchingEntities = new Dictionary<string, LinkedList<Entity>>();

        public void Touched(Entity entity, IEnumerable<string> tags)
        {
            tags.Foreach(t => Touched(entity, t));
        }
         
        public void Touched(Entity entity, string tag)
        {
            if (!TouchingCounts.ContainsKey(tag)) TouchingCounts.Add(tag, 0);
            if (!TouchingEntities.ContainsKey(tag)) TouchingEntities.Add(tag, new LinkedList<Entity>());

            TouchingCounts[tag]++;
            TouchingEntities[tag].AddLast(entity);
        }

        public void Seperated(Entity entity, IEnumerable<string> tags)
        {
            tags.Foreach(t=>Seperated(entity, t));
        }

        public void Seperated(Entity entity, string tag)
        {
            TouchingCounts[tag]--;
            TouchingEntities[tag].Remove(entity);
        }

        public bool IsTouching(string tag)
        {
            return TouchingCounts.ContainsKey(tag) && TouchingCounts[tag] > 0;
        }

        public LinkedList<Entity> TouchingTag(string tag)
        {
            return TouchingEntities.ContainsKey(tag) ? TouchingEntities[tag] : null;
        }
    }
}
