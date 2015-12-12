using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class ScreenWrapper : IKnowsGameObject, IUpdateable, IStartable
    {
        public Vector2 Min;
        public Vector2 Max;

        public GameObject GameObject { get; set; }

        private Sprite renderer;

        public void Update(float step)
        {
            var halfSize = renderer.TextureSize.HalfSize;

            if (GameObject.Transform.LocalPosition.X < Min.X - halfSize.X)
                GameObject.Transform.LocalPosition.X = Max.X + halfSize.X;
            if (GameObject.Transform.LocalPosition.X > Max.X + halfSize.X)
                GameObject.Transform.LocalPosition.X = Min.X - halfSize.X;

            if (GameObject.Transform.LocalPosition.Y < Min.Y - halfSize.Y)
                GameObject.Transform.LocalPosition.Y = Max.Y + halfSize.Y;
            if (GameObject.Transform.LocalPosition.Y > Max.Y + halfSize.Y)
                GameObject.Transform.LocalPosition.Y = Min.Y - halfSize.Y;
        }

        public void Start()
        {
            renderer = GameObject.GetComponent<Sprite>();

            Min = Vector2.Zero;
            Max = new Vector2(Game1.Game.Device.Viewport.Width, Game1.Game.Device.Viewport.Height)*Transform.METRES_A_PIXEL;
        }
    }
}
