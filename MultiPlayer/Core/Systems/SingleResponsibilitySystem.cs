using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class SingleResponsibilitySystem<TMessageType> : ISystem, IKnowsEngine, IHearsMessageTypes
    where TMessageType : class, IMessage
    {
        public virtual void RecieveMessage(IMessage message)
        {
            Handle(message as TMessageType);
        }

        protected abstract void Handle(TMessageType message);

        public Engine Engine { get; set; }
        public IList<Type> HearsMessages { get; } = new List<Type>() { typeof(TMessageType) };
    }

    public abstract class SingleResponsibilitySystem<TMessageType, TFamilyType> : SingleResponsibilitySystem<TMessageType>, IRequiresFamily
        where TMessageType : class, IMessage
    {
        public Type FamilyType { get; } = typeof(TFamilyType);
    }
}
