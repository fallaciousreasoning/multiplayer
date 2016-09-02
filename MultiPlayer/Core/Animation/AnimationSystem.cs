using System;
using System.Collections.Generic;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;

namespace MultiPlayer.Core.Animation
{
    [HearsMessage(typeof(UpdateMessage), typeof(StartAnimationMessage), typeof(StopAnimationMessage), typeof(PauseAnimationMessage))]
    public class AnimationSystem : EntityProcessingSystem
    {
        public void Start(StartAnimationMessage message)
        {
            var animationContainer = message.Target.Get<AnimationContainer>();
            var animation = animationContainer.Animation[message.Animation];

            Start(message.Target, animationContainer, animation);
        }

        public void Start(Entity entity, AnimationContainer container, Animation animation)
        {
            container.CurrentAnimation = animation;
            animation.Start();

            Engine.MessageHub.SendMessage(new AnimationStartedMessage(entity, container, animation));
            container.WasPlaying = true;
        }

        public void Stop(StopAnimationMessage message)
        {
            var container = message.Target.Get<AnimationContainer>();

            var animation = container.CurrentAnimation;
            container.CurrentAnimation?.Stop();
            
            container.CurrentAnimation = null;

            BroadcastFinish(message.Target, container, animation);

            container.WasPlaying = false;
        }

        public void Pause(PauseAnimationMessage message)
        {
            
        }

        protected void Update(Time time, Entity entity, AnimationContainer container)
        {
            if (container.CurrentAnimation == null) return;

            container.CurrentAnimation.Update(time.GameSpeed);

            if (container.WasPlaying && !container.CurrentAnimation.IsPlaying)
            {
                BroadcastFinish(entity, container, container.CurrentAnimation);
            }

            container.WasPlaying = container.CurrentAnimation.IsPlaying;
        }

        private void BroadcastFinish(Entity entity, AnimationContainer container, Animation animation)
        {
            Engine.MessageHub.SendMessage(new AnimationFinishedMessage(entity, container, animation));
        }

        protected override void Process(IMessage message, Entity entity)
        {
            if (message is UpdateMessage)
                Update((message as UpdateMessage).Time, entity, entity.Get<AnimationContainer>());

            if (message is StartAnimationMessage)
                Start(message as StartAnimationMessage);

            if (message is StopAnimationMessage)
                Stop(message as StopAnimationMessage);

            if (message is PauseAnimationMessage)
                Pause(message as PauseAnimationMessage);

        }

        public override IList<Type> Types { get; } = new List<Type>() { typeof(AnimationContainer)};
    }
}
