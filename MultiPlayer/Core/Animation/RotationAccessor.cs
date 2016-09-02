using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Animation
{
    public class RotationAccessor : IAccessor
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
            return Transform.Rotation;
        }

        public void Set(object value, object relativeTo)
        {
            var v1 = (float)value;
            var v2 = (float)(relativeTo ?? 0f);

            Transform.Rotation = v2 + v1;
        }
    }
}
