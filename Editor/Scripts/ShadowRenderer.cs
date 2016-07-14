using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer;
using IDrawable = MultiPlayer.GameComponents.IDrawable;

namespace Editor.Scripts
{
    public class ShadowRenderer : IDrawable
    {
        public Color Color = new Color(Color.White, 0.8f);
        public Texture2D Texture = TextureUtil.CreateTexture((int)Transform.PIXELS_A_METRE, (int)Transform.PIXELS_A_METRE, Color.Gray);
        public Placer Placer { get; set; }

        public void Draw()
        {
            if (!Placer.Dragging) return;

            var size = Placer.Size;
            Game1.Game.SpriteBatch.Draw(Texture, Placer.TopLeft*Transform.PIXELS_A_METRE, null, Color, 0f, Vector2.Zero, size, SpriteEffects.None, 0);
        }
    }
}
