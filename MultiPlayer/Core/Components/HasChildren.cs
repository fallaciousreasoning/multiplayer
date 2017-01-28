using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Components
{
    public class HasChildren : IKnowsEntity
    {
        public IReadOnlyList<Entity> Children => children;
        private readonly List<Entity> children = new List<Entity>();


        public void AddChild(Entity child)
        {
            if (!child.HasComponent<HasParent>())
            {
                child.Add(new HasParent() {Parent = Entity});
                children.Add(child);
                return;
            }

            var parent = child.Get<HasParent>();
            if (parent.Parent == Entity)
            {
                children.Add(child);
                return;
            }

            if (parent.Parent != null)
            {
                var childParent = parent.Parent.Get<HasChildren>();
                childParent.RemoveChild(child);
            }

            children.Add(child);
            parent.Parent = Entity;
        }

        public void RemoveChild(Entity child)
        {
            child.Get<HasParent>().Parent = null;
            children.Remove(child);
        }

        public Entity Entity { get; set; }
    }
}
