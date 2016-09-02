using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Animation.Messages
{
    public class StartAnimationMessage : ITargetedMessage
    {
        public StartAnimationMessage(Entity target, string animation)
        {
            Animation = animation;
            Target = target;
        }

        public Entity Target { get; }
        public string Animation { get; }
    }
}
