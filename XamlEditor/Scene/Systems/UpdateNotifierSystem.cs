using System;
using System.Collections.Generic;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using XamlEditor.Scene.Messages;

namespace XamlEditor.Scene.Systems
{
    public delegate void ComponentEvent(Entity entity, object script);
    public delegate void ChildEvent(Entity entity, Entity child);

    [EditorIgnore]
    [HearsMessage(typeof(EntityAddedMessage))]
    [HearsMessage(typeof(EntityRemovedMessage))]
    [HearsMessage(typeof(ComponentAddedMessage))]
    [HearsMessage(typeof(ComponentRemovedMessage))]
    [HearsMessage(typeof(ComponentChangedMessage))]
    public class UpdateNotifier : IKnowsEngine, ISystem
    {
        public event ComponentEvent ComponentAdded;
        public event ChildEvent ChildAdded;

        public event ComponentEvent ComponentRemoved;
        public event ChildEvent ChildRemoved;

        public event ComponentEvent ComponentChanged;

        private void OnComponentAdded(Entity entity, object o)
        {
            ComponentAdded?.Invoke(entity, o);
        }

        private void OnComponentRemoved(Entity entity, object o)
        {
            ComponentRemoved?.Invoke(entity, o);
        }

        private void OnEntityRemoved(Entity child)
        {
            ChildRemoved?.Invoke(null, child);
        }

        private void OnEntityAdded(Entity child)
        {
            ChildAdded?.Invoke(null, child);
        }

        private void OnComponentChanged(Entity target, object component)
        {
            ComponentChanged?.Invoke(target, component);
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

            var cchangedMessage = message as ComponentChangedMessage;
            if (cchangedMessage != null)
            {
                OnComponentChanged(cchangedMessage.Target, cchangedMessage.Component);
            }
        }
    }
}
