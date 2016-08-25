using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.GameComponents;

namespace Runner.Builders
{
    public class PlayerController : IStartable, IUpdateable, IKnowsGameObject
    {
        private CharacterMotor motor;

        public void Start()
        {
            motor = GameObject.GetComponent<CharacterMotor>();
        }

        public void Update(float step)
        {
            var x = Scene.ActiveScene.Input.GetAxis("horizontal");
            if (x < 0) motor.AccelerateLeft();
            if (x > 0) motor.AccelerateRight();

            if (Scene.ActiveScene.Input.GetButtonDown("jump"))
                motor.Jump();

            if (Scene.ActiveScene.Input.GetButtonDown("slide"))
                motor.Slide();
        }

        public GameObject GameObject { get; set; }
    }
}
