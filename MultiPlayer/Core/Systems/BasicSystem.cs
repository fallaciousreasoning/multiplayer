using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Systems
{
    public abstract class BasicSystem<T> : ISystem, IRequiresFamily, IKnowsEngine 
    {
        protected BasicSystem()
        {
            FamilyType = typeof(T);
        }

        public virtual void RecieveMessage(IMessage message)
        {
            Handle(message);
        }

        protected abstract void Handle(IMessage message);
        
        public Engine Engine { get; set; }
        public Type FamilyType { get; }
    }

    public abstract class BasicSystem : ISystem, IKnowsEngine
    {
        public virtual void RecieveMessage(IMessage message)
        {
            Handle(message);
        }

        protected abstract void Handle(IMessage message);

        public Engine Engine { get; set; }
    }
}
