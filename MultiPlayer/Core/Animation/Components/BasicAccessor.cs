using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer.Core;

namespace MultiPlayer.GameComponents.Animation
{
    public class BasicAccessor<T> : IAccessor
    {
        private readonly Func<T> getter;
        private readonly Action<T, T> setter;
        
        public Entity Target { get; set; }

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
