using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class DrawableSystem : BasicSystem
    {
        protected DrawableSystem(IEnumerable<Type> requires)
            : base(new[] {typeof(DrawMessage)}, requires)
        {
        }

        protected override void Handle(IMessage message)
        {
            var drawMessage = message as DrawMessage;
            if (drawMessage != null)
                Draw();
        }

        protected abstract void Draw();
    }
}

