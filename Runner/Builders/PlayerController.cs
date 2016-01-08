﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var x = Game1.Game.Input.GetAxis("horizontal");
            if (x < 0) motor.AccelerateLeft();
            if (x > 0) motor.AccelerateRight();

            if (Game1.Game.Input.GetButtonDown("jump"))
                motor.Jump();
        }

        public GameObject GameObject { get; set; }
    }
}