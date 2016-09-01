using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class EntityProcessingSystem<T1, T2> 
        : ISystem where T2 : class where T1 : class
    {
        public void RecieveMessage(IMessage message)
        {
            foreach (var entity in new List<Entity>())
                Process(entity.Get<T1>(), entity.Get<T2>());
        }

        public abstract void Process(T1 component1, T2 component2);
    }
}
