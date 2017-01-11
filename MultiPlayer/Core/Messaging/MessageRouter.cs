using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
    public class MessageRouter
    {
        private readonly Dictionary<Type, LinkedList<MessageHandler>> messageHandlers =
            new Dictionary<Type, LinkedList<MessageHandler>>();

        private readonly Dictionary<Type, LinkedList<object>> extendingSystems =
            new Dictionary<Type, LinkedList<object>>();

        private readonly HashSet<Type> systemTypesSet = new HashSet<Type>();
        private readonly LinkedList<Type> systemTypes = new LinkedList<Type>();

        private bool started;
        private Engine engine;

        public MessageRouter(Engine engine)
        {
            this.engine = engine;
        }

        public void AddRoute<TMessage, TSystem>(Action<TMessage, TSystem> handler)
            where TMessage : class, IMessage
            where TSystem : class
        {
            if (started) throw new Exception("Attempted to add route after adding systems!");

            var messageType = typeof(TMessage);
            var systemType = typeof(TSystem);

            if (!systemTypesSet.Contains(systemType))
            {
                systemTypesSet.Add(systemType);
                systemTypes.AddLast(systemType);
            }

            Action<object, object> wrapper = (messageObj, systemObj) =>
            {
                var message = messageObj as TMessage;
                var system = systemObj as TSystem;
                handler(message, system);
            };
            if (messageHandlers.ContainsKey(messageType))
                messageHandlers.Add(messageType, new LinkedList<MessageHandler>());

            messageHandlers[messageType].AddLast(new MessageHandler() {Handle = wrapper, SystemType = systemType});
        }

        public void OnSystemAdded(object system)
        {
            started = true;
            foreach (var interfaceType in systemTypes)
            {
                if (!system.GetType().IsAssignableFrom(interfaceType)) continue;

                if (!extendingSystems.ContainsKey(interfaceType))
                    extendingSystems.Add(interfaceType, new LinkedList<object>());
                extendingSystems[interfaceType].AddLast(system);
            }
        }

        public void SendMessage(IMessage message)
        {
            var messageType = message.GetType();

            var handlers = messageHandlers[messageType];

            foreach (var handler in handlers)
            {
                var systems = extendingSystems[handler.SystemType];
                foreach (var system in systems)
                {
                    handler.Handle(message, system);
                }
            }
        }
    }
}
