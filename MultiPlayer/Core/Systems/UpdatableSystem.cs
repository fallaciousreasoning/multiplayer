using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class UpdatableSystem : BasicSystem
    {
        protected UpdatableSystem(IEnumerable<Type> requires)
            : base(new [] {typeof(UpdateMessage)}, requires)
        {
        }

        protected override void Handle(IMessage message)
        {
            var updateMessage = message as UpdateMessage;
            if (updateMessage != null)
                Update(updateMessage.Time);
        }

        protected abstract void Update(Time time);
    }
}
