using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class StateTransitionMessage : ITargetedMessage
    {
        public StateTransitionMessage(Entity entity, string targetState)
        {
            Target = entity;
            TargetState = targetState;

        }
        public Entity Target { get; }
        public string TargetState { get; }
    }
}
