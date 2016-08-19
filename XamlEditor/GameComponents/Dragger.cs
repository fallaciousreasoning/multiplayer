using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;
using IDrawable = MultiPlayer.GameComponents.IDrawable;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace XamlEditor.GameComponents
{
    public class Dragger : IStartable, ILateUpdateable, IDrawable, IKnowsGameObject
    {
        private static readonly Texture2D DrawTool = TextureUtil.CreateTexture(1, 1, Color.White);

        public GameObject Drag { get; set; }

        public float Thickness { get; set; } = 0.2f;
        public float Length { get; set; } = 2f;

        public Color XAxisColor = Color.Green;
        public Color YAxisColor = Color.Blue;

        private AABB xDragBounds;
        private AABB yDragBounds;

        private bool xDragging;
        private bool yDragging;

        private Vector2 mouseStart;
        private Vector2 startPos;

        public void Start()
        {
            xDragBounds = new AABB()
            {
                Min = new Vector2(-Length, -Thickness/2),
                Max = new Vector2(0)
            };

            yDragBounds = new AABB()
            {
                Min = new Vector2(-Thickness/2, -Length),
                Max = new Vector2(0)
            };
        }

        public void LateUpdate(float step)
        {
            xDragBounds.Centre = GameObject.Transform.Position - xDragBounds.HalfSize;
            yDragBounds.Centre = GameObject.Transform.Position - yDragBounds.HalfSize;

            var input = Scene.ActiveScene.Input;

            var mousePos = input.MousePosition;
            mousePos *= Transform.METRES_A_PIXEL;

            if (input.IsDown(MouseButton.Left) && !xDragging && !yDragging)
            {
                if (xDragBounds.Contains(mousePos))
                    xDragging = true;
                if (yDragBounds.Contains(mousePos))
                    yDragging = true;

                mouseStart = mousePos;
                startPos = GameObject.Transform.Position;
            }

            if (input.IsUp(MouseButton.Left))
            {
                xDragging = false;
                yDragging = false;
                mouseStart = Vector2.Zero;
                startPos = GameObject.Transform.Position;
            }

            var expectedPosition = startPos + (mousePos - mouseStart);
            if (!xDragging)
                expectedPosition.X = startPos.X;
            if (!yDragging)
                expectedPosition.Y = startPos.Y;

            if (xDragging || yDragging)
            {
                GameObject.Transform.Position = expectedPosition;
                if (Drag != null)
                    Drag.Transform.Position = expectedPosition;
            }
        }

        public void Draw()
        {
            var size = new Vector2(Thickness, Length) * Transform.PIXELS_A_METRE;

            Scene.ActiveScene.SpriteBatch.Draw(DrawTool, GameObject.Transform.DrawPosition, null, XAxisColor, MathHelper.PiOver2, new Vector2(0.5f, 0), size, SpriteEffects.None, 0);
            Scene.ActiveScene.SpriteBatch.Draw(DrawTool, GameObject.Transform.DrawPosition, null, YAxisColor, 0, new Vector2(0.5f, 1), size, SpriteEffects.None, 0);
        }

        public GameObject GameObject { get; set; }
    }
}
