using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;

namespace MultiPlayer.Core.Systems
{
    public class SpriteRenderer : DrawableSystem
    {
        public SpriteRenderer()
            : base(new [] {typeof(SpriteFamily)})
        {
        }

        protected override void Draw()
        {
            Scene.ActiveScene.SpriteBatch.Begin();

            var family = Engine.FamilyManager.Get(typeof(SpriteFamily));
            foreach (var drawable in family.Entities)
            {
                var position = drawable.Get<PositionComponent>();
                var sprite = drawable.Get<SpriteComponent>();

                Scene.ActiveScene.SpriteBatch.Draw(sprite.Texture, position.WorldPosition, null, sprite.Tint,
                    position.WorldRotation, sprite.Origin, position.WorldScale, sprite.Effects, 0);
            }

            Scene.ActiveScene.SpriteBatch.End();
        }
    }
}
