using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;

namespace MultiPlayer.Core.Nodes
{
    public class CameraNode : ICanLoad<CameraNode>
    {
        public Camera Camera;
        public Transform Transform;

        public void LoadFrom(Entity source)
        {
            Transform = source.Get<Transform>();
            Camera = source.Get<Camera>();
        }
    }
}
