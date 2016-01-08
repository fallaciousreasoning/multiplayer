using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class SimpleController : IUpdateable, IKnowsGameObject
    {
        public float Speed { get; set; } = 3;
        public void Update(float step)
        {
            var dir = new Vector2(Game1.Game.Input.GetAxis("horizontal"), Game1.Game.Input.GetAxis("vertical"));
            GameObject.Transform.Position += dir*Speed*step;

            if (Game1.Game.Input.IsPressed(MouseButton.Left)) GameObject.Transform.Scale *= 2;
            if (Game1.Game.Input.IsPressed(MouseButton.Right)) GameObject.Transform.Scale *= 0.5f;
        }

        public GameObject GameObject { get; set; }
    }
}
