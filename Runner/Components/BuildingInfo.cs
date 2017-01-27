using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core.Components;

namespace Runner.Components
{
    public class BuildingInfo
    {
        public Color Tint { get; set; } = Color.White;

        public const float MAX_Z = 100;
        public float Z { get; set; }

        public int WidthInTiles { get; set; }
        public int HeightInTiles { get; set; }
        public float Width => WidthInTiles * TileSize;
        public float Height => HeightInTiles * TileSize;

        public Texture2D TopLeftCorner { get; set; }
        public Texture2D TopTexture { get; set; }
        public Texture2D InnerTexture { get; set; }
        public Texture2D WallTexture { get; set; }
        public Texture2D WindowTexture { get; set; }
        public float TileSize { get; set; } = 1;

        public float WindowDensity { get; set; } = 0.1f;
        public bool RandomWindows { get; set; } = true;

        public float TextureXScale => TopTexture.Width * Transform.METRES_A_PIXEL / TileSize;
        public float TextureYScale => InnerTexture.Height * Transform.METRES_A_PIXEL / TileSize;

        public Vector2 Scale => new Vector2(TextureXScale, TextureYScale) * Z / MAX_Z;

        public bool IsWindow(int x, int y)
        {
            return false;
        }

        private void PositionWindows()
        {
            
        }
    }
}
