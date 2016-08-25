using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public class UpdateSystem : IRegistrableSystem
    {
        public void RecieveMessage(IMessage message)
        {
        }

        public IEnumerable<Type> Receives { get; } = new[] {typeof(UpdateMessage)};
    }
}
