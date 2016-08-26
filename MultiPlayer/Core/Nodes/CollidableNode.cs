using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;

namespace MultiPlayer.Core.Nodes
{
    public class CollidableNode : ICanLoad<CollidableNode>
    {
        public Transform Transform;
        public Collider Collider;

        public void LoadFrom(Entity source)
        {
            Transform = source.Get<Transform>();
            Collider = source.Get<Collider>();
        }
    }
}
