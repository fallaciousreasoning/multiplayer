using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using MultiPlayer.Test.Components;

namespace MultiPlayer.Test.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
    public class StayOnMouseSystem : ComponentProcessingSystem<Transform, StayOnMouse>
    {
        protected void Update(Time time, Transform transform, StayOnMouse stayOnMouse)
        {
            var input = Engine.Scene.Input;
            transform.Position = input.MousePosition;
        }

        protected override void Process(IMessage message, Transform component1, StayOnMouse component2)
        {
            if (message is UpdateMessage)
                Update((message as UpdateMessage).Time, component1, component2);
        }
    }
}
