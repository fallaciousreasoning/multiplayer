using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class SubSystem<T> : ISystem where T : IMessage
    {
        public void RecieveMessage(IMessage message)
        {
            //Just trust this has been handled by the composite system
            Handle((T)message);
        }

        public abstract void Handle(T message);
    }
}
