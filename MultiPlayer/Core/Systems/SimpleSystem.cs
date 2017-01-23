using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Systems
{
    public abstract class SimpleSystem<T>
    {
        public virtual void OnEntityAdded(Entity entity)
        {

        }

        public virtual void OnEntityRemoved(Entity entity)
        {

        }

        public virtual void OnComponentAdded(Entity entity, object component)
        {

        }

        public virtual void OnComponentRemoved(Entity entity, object component)
        {

        }

        public virtual void OnCollisionEnter(T node1, T node2)
        {

        }

        public virtual void OnCollisionExit(T node1, T node2)
        {

        }

        public virtual void OnTriggerEnter(T node1, T node2)
        {

        }

        public virtual void OnTriggerExit(T node1, T node2)
        {

        }

        public virtual void Start(T node)
        {

        }

        public virtual void Update(T node)
        {

        }

        public virtual void Draw(T node)
        {

        }
    }
}
