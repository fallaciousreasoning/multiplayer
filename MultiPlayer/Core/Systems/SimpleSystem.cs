using MultiPlayer.Collections;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Extensions;
using MultiPlayer.Core.Animation.Messages;

namespace MultiPlayer.Core.Systems
{
    public abstract class SimpleSystem<T> : ISystem, IKnowsEngine, IHearsMessageTypes, IRequiresFamily
    {
        private readonly Dictionary<Type, Action<IMessage>> messageHandlers = new Dictionary<Type, Action<IMessage>>();

        protected IObservableLinkedList<T> Nodes => NodeFamily.Nodes;

        protected IObservableLinkedList<Entity> Entities { get; private set; }

        public Engine Engine { get; set; }
        public INodeFamily<T> NodeFamily { get; private set; }

        public IList<Type> HearsMessages => new List<Type>
        {
            typeof(ComponentAddedMessage),
            typeof(ComponentRemovedMessage),
            typeof(CollisionMessage),
            typeof(StartMessage),
            typeof(UpdateMessage),
            typeof(DrawMessage)
        };

        public Type FamilyType => typeof(T);

        public virtual void OnNodeAdded(Entity entity, T node)
        {

        }

        public virtual void OnNodeRemoved(Entity entity, T node)
        {

        }

        public virtual void OnComponentAdded(Entity entity, T node, object component)
        {

        }

        public virtual void OnComponentRemoved(Entity entity, T node, object component)
        {

        }

        public virtual void OnCollisionEnter(Entity entity1, Entity entity2, T node1, T node2)
        {

        }

        public virtual void OnCollisionExit(Entity entity1, Entity entity2, T node1, T node2)
        {

        }

        public virtual void OnTriggerEnter(Entity entity1, Entity entity2, T node1, T node2)
        {

        }

        public virtual void OnTriggerExit(Entity entity1, Entity entity2, T node1, T node2)
        {

        }

        public virtual void OnAnimationStarted(Entity entity, T node)
        {

        }

        public virtual void OnAnimationFinished(Entity entity, T node)
        {

        }

        public virtual void StartSystem()
        {
            Entities = Engine.FamilyManager.Get(this.RequiredTypes()).Entities;
            NodeFamily = Engine.FamilyManager.GetNodeFamily<T>();

            Entities.ItemAdded += (o, e) =>
            {
                var entity = (Entity)e;
                OnNodeAdded(entity, NodeFamily.NodeForEntity(entity));
            };
            Entities.ItemRemoved += (o, e) =>
            {
                var entity = (Entity)e;
                OnNodeRemoved(entity, NodeFamily.NodeForEntity(entity));
            };
        }

        public virtual void Start(Entity entity, T node)
        {

        }

        public virtual void UpdateSystem()
        {

        }

        public virtual void Update(Entity entity, T node)
        {

        }

        public virtual void DrawSystem()
        {

        }

        public virtual void Draw(Entity entity, T node)
        {

        }

        public virtual void OnUnhandledMessage(IMessage message)
        {

        }

        public void RecieveMessage(IMessage message)
        {
            if (message is StartMessage)
            {
                StartSystem();
                Entities.ForEach(e =>
                {
                    Start(e, NodeFamily.NodeForEntity(e));
                });
            }
            else if (message is UpdateMessage)
            {
                UpdateSystem();
                Entities.ForEach(e => {
                    Update(e, NodeFamily.NodeForEntity(e));
                });
            }
            else if (message is DrawMessage)
            {
                DrawSystem();
                Entities.ForEach(e => {
                    Draw(e, NodeFamily.NodeForEntity(e));
                });
            }
            else if (message is ComponentAddedMessage)
            {
                var m = (ComponentAddedMessage)message;
                OnComponentAdded(m.Target, NodeFamily.NodeForEntity(m.Target), m.Component);
            }
            else if (message is ComponentRemovedMessage)
            {
                var m = (ComponentRemovedMessage)message;
                OnComponentRemoved(m.Target, NodeFamily.NodeForEntity(m.Target), m.Component);
            }
            else if (message is CollisionMessage)
            {
                var m = (CollisionMessage)message;
                if (m.IsTrigger)
                {
                    if (m.Mode == CollisionMode.Entered)
                        OnTriggerEnter(m.Hit, m.Target, NodeFamily.NodeForEntity(m.Target), NodeFamily.NodeForEntity(m.Hit));
                    else OnTriggerExit(m.Hit, m.Target, NodeFamily.NodeForEntity(m.Target), NodeFamily.NodeForEntity(m.Hit));
                }
                else
                {
                    if (m.Mode == CollisionMode.Entered)
                        OnCollisionEnter(m.Hit, m.Target, NodeFamily.NodeForEntity(m.Target), NodeFamily.NodeForEntity(m.Hit));
                    else OnCollisionExit(m.Hit, m.Target, NodeFamily.NodeForEntity(m.Target), NodeFamily.NodeForEntity(m.Hit));
                }
            }
            else if (message is AnimationStartedMessage)
            {
                var m = message as AnimationStartedMessage;
                OnAnimationStarted(m.Target, NodeFamily.NodeForEntity(m.Target));
            }
            else if (message is AnimationFinishedMessage)
            {
                var m = message as AnimationFinishedMessage;
                OnAnimationFinished(m.Target, NodeFamily.NodeForEntity(m.Target));
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
