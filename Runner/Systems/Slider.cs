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
    [HearsMessage(typeof(UpdateMessage))]
    public class Slider : EntityProcessingSystem
    {
        protected override void OnEntityAdded(Entity entity)
        {
            var startSlidingMessage = new StartAnimationMessage(entity, Animations.Name(PlayerAnimation.SlideDown, entity.Get<CharacterInfo>().Facing));
            Engine.MessageHub.SendMessage(startSlidingMessage);

            base.OnEntityAdded(entity);
        }

        private void Update(float time, Entity entity, CharacterStats stats, CharacterInput input, CharacterInfo info, Slide slide)
        {
            var direction = (info.Facing == Direction.Left) ? -1 : 1;

            info.Velocity.X -= info.Velocity.X*stats.HorizontalSlideDrag*time;
            
            if (Math.Abs(info.Velocity.X) - stats.MinSlideVelocity < 0)
            {
                if (info.CanStand) StartStand(entity, info, slide);
                else info.Velocity.X = direction*stats.MinSlideVelocity;
            }

            if (input.Jump)
            {
                StartStand(entity, info, slide);
                input.Jump = false;
            }
        }

        private void StartStand(Entity entity, CharacterInfo info, Slide slide)
        {
            if (info.TillJump > 0 || !info.CanStand || slide.AnimatingUp || !slide.AnimatedDown) return;

            var startAnimationMessage = new StartAnimationMessage(entity, Animations.Name(PlayerAnimation.SlideUp, info.Facing));
            Engine.MessageHub.SendMessage(startAnimationMessage);

            slide.AnimatingUp = true;
        }

        private void Complete(Entity entity)
        {
            entity.Remove<Slide>();
            entity.Add<Move>();

            entity.Get<CharacterInput>().Slide = false;
        }

        protected override void Process(IMessage message, Entity entity)
        {
            var updateMessage = message as UpdateMessage;
            if ((updateMessage != null))
                Update(updateMessage.Time.Step, entity, entity.Get<CharacterStats>(), entity.Get<CharacterInput>(), entity.Get<CharacterInfo>(), entity.Get<Slide>());


            if (message is AnimationFinishedMessage)
            {
                var slide = entity.Get<Slide>();
                if (slide.AnimatingUp)
                    Complete(entity);
                else if (!slide.AnimatedDown)
                    slide.AnimatedDown = true;
            }
        }

        public override IList<Type> Types { get; } = new List<Type>()
        {
            typeof(CharacterStats),
            typeof(CharacterInput),
            typeof(CharacterInfo),
            typeof(AnimationContainer),
            typeof(Slide),
        };
    }
}
