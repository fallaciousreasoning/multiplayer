using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Systems;
using Runner.Components;

namespace Runner.Systems
{
    public class BuildingRenderer : SimpleSystem<(BuildingInfo info, Transform transform)>
    {
        public override void DrawSystem()
        {
            var spriteBatch = Scene.ActiveScene.SpriteBatch;
            spriteBatch.Begin(0, null, null, null, null, null, Engine.Systems.Get<CameraSystem>().CameraWorld);

            foreach (var node in Nodes)        
                DrawBuilding(spriteBatch, node.info, node.transform);
            
            spriteBatch.End();
            base.DrawSystem();
        }

        private void DrawBuilding(SpriteBatch spriteBatch, BuildingInfo info, Transform transform)
        {
            var step = info.TileSize * info.PositionScale;
            var halfSize = info.TileSize * 0.5f * info.PositionScale;
            var halfWidth = info.Width * 0.5f * info.PositionScale;
            var height = info.Height * info.PositionScale;

            for (int x = 0; x < info.WidthInTiles; ++x)
            for (var y = 0; y < info.HeightInTiles; ++y)
            {
                var xPos = transform.Position.X - halfWidth + step * x;
                var yPos = transform.Position.Y - height + step * y;
                var position = new Vector2(xPos, yPos + transform.WorldPosition.Y*info.PositionScale) * Transform.PIXELS_A_METRE ;

                var texture = info.InnerTexture;
                var effects = SpriteEffects.None;

                if (y == 0)
                {
                    if (x == 0)
                        texture = info.TopLeftCorner;
                    else if (x == info.WidthInTiles - 1)
                    {
                        texture = info.TopLeftCorner;
                        effects = SpriteEffects.FlipHorizontally;
                    }
                    else texture = info.TopTexture;
                }
                else if (x == 0)
                    texture = info.WallTexture;
                else if (x == info.WidthInTiles - 1)
                {
                    texture = info.WallTexture;
                    effects = SpriteEffects.FlipHorizontally;
                }
                else if (info.IsWindow(x, y))
                {
                    texture = info.WindowTexture;
                } 

                spriteBatch.Draw(texture, position, null, info.Tint, 0, new Vector2(halfSize), info.Scale, effects, info.Z);
            }
        }
    }
}
