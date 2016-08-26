using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Systems
{
    public abstract class BasicSystem<T> : IRegistrableSystem, IRequiresFamily, IKnowsEngine 
    {
        protected readonly HashSet<Type> CanReceive = new HashSet<Type>();

        protected BasicSystem(IEnumerable<Type> canRecieve)
        {
            canRecieve.Foreach(c => CanReceive.Add(c));
            FamilyType = typeof(T);
        }

        public virtual void RecieveMessage(IMessage message)
        {
            var type = message.GetType();
            if (!CanReceive.Contains(type)) return;

            Handle(message);
        }

        protected abstract void Handle(IMessage message);

        public IEnumerable<Type> Receives => CanReceive;
        public Engine Engine { get; set; }
        public Type FamilyType { get; }
    }
}
