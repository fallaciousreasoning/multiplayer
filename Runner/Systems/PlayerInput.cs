using System;
using MultiPlayer;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using Runner.Builders;
using Runner.Components;

namespace Runner.Systems
{
    [HearsMessage(typeof(UpdateMessage))]
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

            characterInput.Direction = 0;

            if (x < 0) characterInput.Direction--;
            if (x > 0) characterInput.Direction++;

            if (input.GetButtonDown("jump"))
                characterInput.Jump = true;

            if (Scene.ActiveScene.Input.GetButtonDown("slide"))
                characterInput.Slide = true;
        }
    }
}
