using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(LateUpdateMessage))]
    public abstract class LateUpdatableSystem<T> : BasicSystem<T>
    {
        protected LateUpdatableSystem()
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
