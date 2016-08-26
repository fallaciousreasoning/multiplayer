using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class LateUpdateMessage : IMessage
    {
        public LateUpdateMessage(Time time)
        {
            Time = time;
        }

        public Time Time { get; }
    }
}
