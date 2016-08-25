using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Systems;

namespace MultiPlayer.Core.Messaging
{
    public class UpdateMessage : IMessage
    {
        public UpdateMessage(Time time)
        {
            Time = time;
        }

        public Time Time { get; }
    }
}
