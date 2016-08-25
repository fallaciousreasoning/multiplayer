using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public interface ITargetedMessage : IMessage
    {
        Entity Target { get; }
    }
}
