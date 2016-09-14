using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    [HearsMessage(typeof(AnimationFinishedMessage))]
    public class Diver : EntityProcessingSystem
    {
        protected override void OnEntityAdded(Entity entity)
        {
            Dive(entity);
            base.OnEntityAdded(entity);
        }

        private void Dive(Entity entity)
        {
            var startAnimationMessage = new StartAnimationMessage(entity,
                Animations.Name(PlayerAnimation.Dive, entity.Get<CharacterInfo>().Facing));
            Engine.MessageHub.SendMessage(startAnimationMessage);
        }

        protected override void Process(IMessage message, Entity entity)
        {
            var updateMessage = message as UpdateMessage;
            if (updateMessage != null)
            {
                var info = entity.Get<CharacterInfo>();

                //If we hit the ground, roll
                if (info.OnGround)
                {
                    Engine.MessageHub.SendMessage(new StopAnimationMessage(entity));

                    entity.Get<Transform>().Rotation = 0;

                    entity.Remove<Dive>();
                    entity.Add<Roll>();
                }
            }
        }

        public override IList<Type> Types { get; } = new List<Type>()
        {
            typeof(Transform),
            typeof(CharacterStats),
            typeof(CharacterInfo),
            typeof(AnimationContainer),
            typeof(Dive),
        };
    }
}
