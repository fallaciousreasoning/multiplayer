using System;
using System.Collections.Generic;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;

namespace XamlEditor.Scene.Systems
{
    public delegate void ComponentEvent(Entity entity, object script);
    public delegate void ChildEvent(Entity entity, Entity child);

    [EditorIgnore]
    [HearsMessage(typeof(EntityAddedMessage))]
    [HearsMessage(typeof(EntityRemovedMessage))]
    [HearsMessage(typeof(ComponentAddedMessage))]
    [HearsMessage(typeof(ComponentRemovedMessage))]
    public class UpdateNotifier : IKnowsEngine, ISystem
    {
        public event ComponentEvent ComponentAdded;
        public event ChildEvent ChildAdded;

        public event ComponentEvent ComponentRemoved;
        public event ChildEvent ChildRemoved;

        public void OnComponentAdded(Entity entity, object o)
        {
            ComponentAdded?.Invoke(entity, o);
        }

        public void OnComponentRemoved(Entity entity, object o)
        {
            ComponentRemoved?.Invoke(entity, o);
        }

        public void OnEntityRemoved(Entity child)
        {
            ChildRemoved?.Invoke(null, child);
        }

        public void OnEntityAdded(Entity child)
        {
            ChildAdded?.Invoke(null, child);
        }

        public IList<Type> Types { get; } = new List<Type>() {typeof(Transform)};
        public Engine Engine { get; set; }

        public void RecieveMessage(IMessage message)
        {
            var addedMessage = message as EntityAddedMessage;
            if (addedMessage != null)
            {
                OnEntityAdded(addedMessage.Target);
            }

            var removedMessage = message as EntityAddedMessage;
            if (removedMessage != null)
            {
                OnEntityRemoved(removedMessage.Target);
            }

            var caddedMessage = message as ComponentAddedMessage;
            if (caddedMessage != null)
            {
                OnComponentAdded(caddedMessage.Target, caddedMessage.Component);
            }

            var cremovedMessage = message as ComponentRemovedMessage;
            if (cremovedMessage != null)
            {
                OnComponentRemoved(cremovedMessage.Target, cremovedMessage.Component);
            }
        }
    }
}
