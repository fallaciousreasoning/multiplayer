using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Systems
{
    public abstract class BasicSystem : IRegistrableSystem, IRequiresFamilies, IKnowsEngine
    {
        protected readonly HashSet<Type> CanReceive = new HashSet<Type>();
        protected readonly HashSet<Type> Requires = new HashSet<Type>();

        protected BasicSystem(IEnumerable<Type> canRecieve, IEnumerable<Type> requires)
        {
            canRecieve.Foreach(c => CanReceive.Add(c));
            requires.Foreach(c => Requires.Add(c));
        }

        public virtual void RecieveMessage(IMessage message)
        {
            var type = message.GetType();
            if (!CanReceive.Contains(type)) return;

            Handle(message);
        }

        protected abstract void Handle(IMessage message);

        public IEnumerable<Type> Receives => CanReceive;
        public IEnumerable<Type> RequiredFamilies => Requires;
        public Engine Engine { get; set; }
    }
}
