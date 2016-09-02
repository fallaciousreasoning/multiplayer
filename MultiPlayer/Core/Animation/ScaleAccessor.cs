using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Animation
{
    public class ScaleAccessor : IAccessor
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
            return Transform.Scale;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (Vector2)value;
            var v2 = (Vector2)(relativeTo ?? Vector2.One);

            Transform.Scale = v2 * v1;
        }
    }
}
