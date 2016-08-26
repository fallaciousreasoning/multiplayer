using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class LateUpdatableSystem : BasicSystem
    {
        protected LateUpdatableSystem(IEnumerable<Type> requires)
            : base(new [] {typeof(LateUpdateMessage)}, requires)
        {
        }

        protected override void Handle(IMessage message)
        {
            var updateMessage = message as LateUpdateMessage;
            if (updateMessage != null)
                LateUpdate(updateMessage.Time);
        }

        protected abstract void LateUpdate(Time time);
    }
}
