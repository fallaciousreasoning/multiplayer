using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(DrawMessage))]
    public abstract class DrawableSystem<T> : BasicSystem<T>
    {
        protected override void Handle(IMessage message)
        {
            var drawMessage = message as DrawMessage;
            if (drawMessage != null)
                Draw();
        }

        protected abstract void Draw();
    }
}

