using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class BasicSystem : ISystem
    {
        public abstract void RecieveMessage(IMessage message);
    }
}
