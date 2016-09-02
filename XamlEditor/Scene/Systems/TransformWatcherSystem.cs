using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using XamlEditor.Scene.Components;
using XamlEditor.Scene.Messages;

namespace XamlEditor.Scene.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public class TransformWatcherSystem : EntityProcessingSystem
    {
        protected void Process(Entity entity, TransformWatcher watch, Transform transform)
        {
            if (watch.Position != transform.Position || watch.Scale != transform.Scale ||
                watch.Rotation != transform.Rotation)
                Engine.MessageHub.SendMessage(new ComponentChangedMessage(entity, transform));

            watch.Position = transform.Position;
            watch.Scale = transform.Scale;
            watch.Rotation = transform.Rotation;
        }

        protected override void Process(IMessage message, Entity entity)
        {
            if (!(message is UpdateMessage)) return;

            Process(entity, entity.Get<TransformWatcher>(), entity.Get<Transform>());
        }

        public override IList<Type> Types { get; } = new List<Type>()
        {
            typeof(TransformWatcher),
            typeof(Transform)
        };
    }
}
