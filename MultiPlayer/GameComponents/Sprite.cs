using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.GameComponents;
using IDrawable = MultiPlayer.GameComponents.IDrawable;

namespace MultiPlayer
{
    public class Sprite : IDrawable, IKnowsGameObject
    {
        private Vector2 origin;
        private Texture2D texture;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public Color Tint = Color.White;

        public AABB TextureSize { get; private set; }

        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                origin = new Vector2(Texture.Width*0.5f, Texture.Height*0.5f);
                TextureSize = AABB.FromTexture(texture);
            }
        }

        public void Draw()
        {
            if (Texture == null) return;

            Scene.ActiveScene.SpriteBatch.Draw(Texture, GameObject.Transform.DrawPosition, null, Tint, GameObject.Transform.Rotation, origin, GameObject.Transform.Scale, Effects, 0f);
        }

        public GameObject GameObject { get; set; }
    }
}
