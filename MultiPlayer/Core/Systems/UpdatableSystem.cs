using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public abstract class UpdatableSystem<T> : BasicSystem<T> 
        where T : class, new()
    {
        protected override void Handle(IMessage message)
        {
            var updateMessage = message as UpdateMessage;
            if (updateMessage != null)
                Update(updateMessage.Time);
        }

        protected abstract void Update(Time time);
    }
}
