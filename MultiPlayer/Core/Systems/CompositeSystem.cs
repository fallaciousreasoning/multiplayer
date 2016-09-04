using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public class CompositeSystem<T> : ISystem, IRequiresFamily, IHearsMessageTypes
    {
        public NodeFamily<T> Nodes { get; private set; }
        private Dictionary<Type, ISystem> Systems { get; } = new Dictionary<Type, ISystem>();

        public void RecieveMessage(IMessage message)
        {
            var messageType = message.GetType();

            if (Systems.ContainsKey(messageType))
                Systems[messageType].RecieveMessage(message);
        }

        public CompositeSystem<T> WithSystem<K>(SubSystem<K> system) where K : IMessage
        {
            Systems.Add(typeof(K), system);
            HearsMessages.Add(typeof(K));
            return this;
        }
        
        public IList<Type> HearsMessages { get; } = new List<Type>();
        public Type FamilyType { get; } = typeof(T);
    }
}
