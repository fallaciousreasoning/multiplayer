using MultiPlayer.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Systems
{
    public abstract class SimpleSystem<T> : ISystem, IKnowsEngine
    {
        private Dictionary<Type, Action<IMessage>> messageHandlers = new Dictionary<Type, Action<IMessage>>();

        protected List<T> Nodes { get; private set; }

        public Engine Engine { get; set; }

        public virtual void OnNodeAdded(T node)
        {

        }

        public virtual void OnNodeRemoved(T node)
        {

        }

        public virtual void OnComponentAdded(Entity node, object component)
        {

        }

        public virtual void OnComponentRemoved(Entity node, object component)
        {

        }

        public virtual void OnCollisionEnter(T node1, T node2)
        {

        }

        public virtual void OnCollisionExit(T node1, T node2)
        {

        }

        public virtual void OnTriggerEnter(T node1, T node2)
        {

        }

        public virtual void OnTriggerExit(T node1, T node2)
        {

        }

        public virtual void Start(T node)
        {

        }

        public virtual void Update(T node)
        {

        }

        public virtual void Draw(T node)
        {

        }

        public virtual void OnUnhandledMessage(IMessage message)
        {

        }

        public void RecieveMessage(IMessage message)
        {
            if (message is StartMessage)
                Nodes.ForEach(Start);
            else if (message is UpdateMessage)
                Nodes.ForEach(Update);
            else if (message is DrawMessage)
                Nodes.ForEach(Draw);
            else if (message is ComponentAddedMessage)
            {
                var m = (ComponentAddedMessage)message;
                OnComponentAdded(m.Target, m.Component);
            }
            else if (message is ComponentRemovedMessage)
            {
                var m = (ComponentRemovedMessage)message;
                OnComponentRemoved(m.Target, m.Component);
            }
            else if (message is CollisionMessage)
            {
                var m = (CollisionMessage)message;
                if (m.IsTrigger)
                {
                    if (m.Mode == CollisionMode.Entered)
                        OnTriggerEnter(m.Target, m.Hit);
                    else OnTriggerExit(m.Target, m.Hit);
                }
                else
                {
                    if (m.Mode == CollisionMode.Entered)
                        OnCollisionEnter(m.Target, m.Hit);
                    else OnCollisionExit(m.Target, m.Hit);
                }
            }
            else if (messageHandlers.ContainsKey(message.GetType()))
                messageHandlers[message.GetType()](message);
            else OnUnhandledMessage(message);
        }

        public void RegisterHandler<MessageType>(Action<MessageType> handler)
            where MessageType : IMessage
        {
            var messageType = typeof(MessageType);
            if (messageHandlers.ContainsKey(messageType)) throw new Exception("There is already a handler for " + messageType.Name);

            Action<IMessage> wrapper = (m) => handler((MessageType)m);
            messageHandlers.Add(messageType, wrapper);
        }
    }
}
