﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class StayOnMouse : IUpdateable, IKnowsGameObject
    {
        public void Update(float step)
        {
            GameObject.Transform.DrawPosition = Game1.Game.Input.MousePosition;
        }

        public GameObject GameObject { get; set; }
    }
}