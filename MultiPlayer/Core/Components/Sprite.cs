using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiPlayer.Core.Components
{
    public class Sprite
    {
        public Texture2D Texture;
        public Color Tint = Color.White;
        public SpriteEffects Effects;

        public Vector2 Origin => new Vector2(Texture.Width, Texture.Height)*0.5f;
    }
}
