using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Animation.Messages
{
    public class AnimationFinishedMessage : IMessage
    {
        public AnimationFinishedMessage(Entity target, AnimationContainer container, Animation animation)
        {
            Target = target;
            Animation = animation;
            Container = container;
        }

        public Entity Target { get; }
        public Animation Animation { get; }
        public AnimationContainer Container { get; }
    }
}
