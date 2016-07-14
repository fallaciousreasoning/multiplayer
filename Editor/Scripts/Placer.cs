using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Editor.Scripts
{
    public class Placer : IUpdateable
    {
        /// <summary>
        /// Called when something should be placed
        /// </summary>
        public Action<Vector2, Vector2> Place;

        public float GridSize = 0.5f;
        public bool Snap { get; set; } = true;

        private InputManager Input { get { return Game1.Game.Input; } }
        private Camera Camera { get { return Camera.ActiveCamera;} }

        public bool Dragging { get; private set; }

        public Vector2 TopLeft
        {
            get { return new Vector2(Math.Min(StartPos.X, EndPos.X), Math.Min(StartPos.Y, EndPos.Y));}
        }

        public Vector2 Size{get{var size = EndPos - StartPos;return new Vector2(Math.Abs(size.X), Math.Abs(size.Y));}}

        public Vector2 Centre { get { return (StartPos + EndPos)*0.5f; } }

        public Vector2 StartPos { get; private set; }
        public Vector2 EndPos { get; private set; }

        public void Update(float step)
        {
            if (Input.IsPressed(MouseButton.Left))
            {
                Dragging = true;
                StartPos = SnapToGrid(WorldPos(Input.MousePosition));
            }

            if (Dragging)
            {
                if (Input.IsReleased(MouseButton.Left))
                {
                    Dragging = false;
                    Place?.Invoke(TopLeft, Size);
                }

                EndPos = SnapToGrid(WorldPos(Input.MousePosition));
            }
        }

        private Vector2 SnapToGrid(Vector2 pos)
        {
            if (!Snap) return pos;

            var result = pos;
            result /= GridSize;

            result.X = (float)Math.Round(result.X);
            result.Y = (float)Math.Round(result.Y);

            result *= GridSize;

            return result;
        }

        private Vector2 WorldPos(Vector2 screenPos)
        {
            return ((screenPos - new Vector2(Game1.Game.GraphicsDevice.Viewport.Width, Game1.Game.GraphicsDevice.Viewport.Height)*0.5f)*Transform.METRES_A_PIXEL)/Camera.GameObject.Transform.Scale + Camera.GameObject.Transform.Position;
        }
    }
}
