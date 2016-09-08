using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(AnimationFinishedMessage))]
    public class Roller : EntityProcessingSystem
    {
        protected override void OnEntityAdded(Entity entity)
        {
            Roll(entity);
            base.OnEntityAdded(entity);
        }

        private void Roll(Entity entity)
        {
            var startAnimationMessage = new StartAnimationMessage(entity,
                Animations.Name(PlayerAnimation.Roll, entity.Get<CharacterInfo>().Facing));
            Engine.MessageHub.SendMessage(startAnimationMessage);
        }

        protected override void Process(IMessage message, Entity entity)
        {
            var animationFinishedMessage = message as AnimationFinishedMessage;
            if (animationFinishedMessage == null) return;

            if (animationFinishedMessage.Target == entity)
            {
                if (!entity.Get<CharacterInfo>().CanStand)
                    Roll(entity);
                else
                {
                    entity.Remove<Roll>();
                    entity.Add<Move>();
                }
            }
        }

        public override IList<Type> Types { get; } = new List<Type>()
        {
            typeof(CharacterStats),
            typeof(CharacterInfo),
            typeof(AnimationContainer),
            typeof(Roll),
        };
    }
}
