using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Systems;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Messaging
{
    public class MessageHub
    {
        private readonly Engine engine;
        private readonly Dictionary<Type, LinkedList<Type>> messageRecievers = new Dictionary<Type, LinkedList<Type>>();

        public MessageHub(Engine engine)
        {
            this.engine = engine;
        }

        public void Register<T, K>()
            where T : ISystem
            where K : IMessage
        {
            Register(typeof(K), typeof(T));
        }

        internal void Register(Type systemType, Type messageType)
        {
            if (!messageRecievers.ContainsKey(messageType))
                messageRecievers.Add(messageType, new LinkedList<Type>());

            messageRecievers[messageType].AddLast(systemType);
        }

        public void BroadcastMessage(IMessage message)
        {
            engine.Systems.Foreach(s => s.RecieveMessage(message));
        }

        public void SendMessage(IMessage message)
        {
            var type = message.GetType();
            var recievers = messageRecievers[type];
            SendMessage(message, recievers);
        }

        public void SendMessage(IMessage message, IEnumerable<Type> to)
        {
            to.Foreach(t => SendMessage(message, t));
        }

        public void SendMessage(IMessage message, Type to)
        {
            var system = engine.Systems.Get(to);
            system.RecieveMessage(message);
        }
    }
}
