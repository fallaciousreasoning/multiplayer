using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer.Core;

namespace MultiPlayer.GameComponents
{
    public class SimpleController : IUpdateable, IKnowsGameObject
    {
        public float Speed { get; set; } = 3;
        public void Update(float step)
        {
            var dir = new Vector2(Scene.ActiveScene.Input.GetAxis("horizontal"), Scene.ActiveScene.Input.GetAxis("vertical"));
            GameObject.Transform.Position += dir*Speed*step;

            if (Scene.ActiveScene.Input.IsPressed(MouseButton.Left)) GameObject.Transform.Scale *= 2;
            if (Scene.ActiveScene.Input.IsPressed(MouseButton.Right)) GameObject.Transform.Scale *= 0.5f;
        }

        public GameObject GameObject { get; set; }
    }
}
