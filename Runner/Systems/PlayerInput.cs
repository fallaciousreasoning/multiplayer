using System;
using MultiPlayer;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Builders;
using Runner.Components;

namespace Runner.Systems
{
    public class PlayerInput : ComponentProcessingSystem<CharacterInput>
    {
        private InputManager input;

        protected override void Start()
        {
            input = Engine.Systems.Get<InputManager>();
            base.Start();
        }

        public override void RecieveMessage(IMessage message)
        {
            if (!(message is UpdateMessage) && Started) return;

            base.RecieveMessage(message);
        }

        protected override void Process(IMessage message, CharacterInput characterInput)
        {
            var x = input.GetAxis("horizontal");
            characterInput.AccelerateLeft = x < 0;
            characterInput.AccelerateRight = x > 0;

            if (input.GetButtonDown("jump"))
                characterInput.Jump = true;

            if (Scene.ActiveScene.Input.GetButtonDown("slide"))
                characterInput.Slide = true;
        }
    }
}
