using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class StateTransitionMessage : ITargetedMessage
    {
        public Entity Target { get; set; }
        public string TargetState { get; set; }
    }
}
