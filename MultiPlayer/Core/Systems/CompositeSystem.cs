using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Collections;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(StartMessage))]
    public class CompositeSystem<T> : ISystem, IRequiresFamily, IHearsMessageTypes, IKnowsEngine
    {
        public INodeFamily<T> Nodes { get; private set; }
        private Dictionary<Type, ISystem> Systems { get; } = new Dictionary<Type, ISystem>();

        protected void Start()
        {
            Nodes = Engine.FamilyManager.GetNodeFamily<T>();
        }

        public void RecieveMessage(IMessage message)
        {
            if (message is StartMessage) Start();

            var messageType = message.GetType();

            if (Systems.ContainsKey(messageType))
                Systems[messageType].RecieveMessage(message);
        }

        public CompositeSystem<T> WithSystem<K>(SubSystem<T, K> system) where K : IMessage
        {
            Systems.Add(typeof(K), system);
            HearsMessages.Add(typeof(K));
            return this;
        }
        
        public IList<Type> HearsMessages { get; } = new List<Type>();
        public Type FamilyType { get; } = typeof(T);
        public Engine Engine { get; set; }
    }
}
