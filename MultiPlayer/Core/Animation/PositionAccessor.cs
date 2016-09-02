using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Animation
{
    public class PositionAccessor : IAccessor
    {
        private Entity entity;

        public Entity Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                Transform = entity.Get<Transform>();
            }
        }

        public Transform Transform { get; private set; }

        public object Get()
        {
            return Transform.Position;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (Vector2) value;
            var v2 = (Vector2) (relativeTo ?? Vector2.Zero);

            Transform.Position = v2 + v1;
        }
    }
}
