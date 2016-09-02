using System;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Animation
{
    public class BasicAccessor<T> : IAccessor
    {
        private readonly Func<T> getter;
        private readonly Action<T, T> setter;


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
            return getter();
        }

        public void Set(object value, object relativeTo)
        {
            setter((T) value, (T) relativeTo);
        }

        public BasicAccessor(Func<T> getter, Action<T, T> setter)
        {
            this.getter = getter;
            this.setter = setter;
        } 
    }
}
