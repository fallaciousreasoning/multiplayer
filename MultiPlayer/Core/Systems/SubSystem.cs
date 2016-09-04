using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class SubSystem<T1, T2> : ISystem where T2 : IMessage
    {
        public CompositeSystem<T1> Owner { get; set; }

        public void RecieveMessage(IMessage message)
        {
            //Just trust this has been handled by the composite system
            Handle((T2)message);
        }

        public abstract void Handle(T2 message);
    }
}
