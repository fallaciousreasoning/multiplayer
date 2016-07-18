using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.Actions;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace Editor.Scripts.Placer
{
    public class Placer : IUpdateable
    {
        public PlacerSettings Settings { get; set; } = new PlacerSettings();
        public ActionManager Manager { get; set; }

        private InputManager Input { get { return Scene.ActiveScene.Input; } }
        private Camera Camera { get { return Camera.ActiveCamera;} }
        
        public Vector2 TopLeft => new Vector2(Math.Min(StartPos.X, EndPos.X), Math.Min(StartPos.Y, EndPos.Y));

        public Vector2 Size
        {
            get
            {
                var size = EndPos - StartPos;
                return new Vector2(Math.Max(Math.Abs(size.X), Settings.MinSize),
                    Math.Max(Math.Abs(size.Y), Settings.MinSize));
            }
        }

        public Vector2 DirectedSize
        {
            get
            {
                var size = EndPos - StartPos;
                var xMul = size.X < 0 ? -1 : 1;
                var yMul = size.Y < 0 ? -1 : 1;

                return Size*new Vector2(xMul, yMul);
            }
        }

        public Vector2 Centre => (StartPos + EndPos)*0.5f;

        public Vector2 StartPos { get; private set; }
        public Vector2 EndPos { get; private set; }

        public bool Dragging { get; private set; }

        public void Update(float step)
        {
            if (Input.IsPressed(MouseButton.Left))
            {
                Dragging = true;
                StartPos = SnapToGrid(WorldPos(Input.MousePosition));
                if (Settings.Shadow != null)
                    Settings.Shadow.Visible = true;
            }

            if (Dragging)
            {
                if (Input.IsReleased(MouseButton.Left))
                {
                    Dragging = false;
                    if (Settings.Shadow != null)
                        Settings.Shadow.Visible = false;

                    Place();
                }

                EndPos = SnapToGrid(WorldPos(Input.MousePosition));
            }

            UpdateShadow();
        }

        private void Place()
        {
            var prefabInfo = new PrefabInfo()
            {
                PrefabName = Settings.PlacingPrefab,
                Position = Settings.NoSize ? EndPos : StartPos + DirectedSize * 0.5f,
                Scale = Settings.NoSize ? Vector2.One : Size,
            };
            Manager.Do(new PlaceBlockAction(prefabInfo));
        }

        private void UpdateShadow()
        {
            if (Settings.Shadow == null) return;

            Settings.Shadow.Transform.Position = Settings.NoSize ? EndPos : StartPos+DirectedSize*0.5f;
            Settings.Shadow.Transform.Scale = Settings.NoSize ? Vector2.One : Size;
        }

        private Vector2 SnapToGrid(Vector2 pos)
        {
            if (!Settings.SnapToGrid) return pos;

            var result = pos;
            result /= Settings.GridSize;

            result.X = (float)Math.Round(result.X);
            result.Y = (float)Math.Round(result.Y);

            result *= Settings.GridSize;

            return result;
        }

        private Vector2 WorldPos(Vector2 screenPos)
        {
            return ((screenPos - new Vector2(Scene.ActiveScene.Device.Viewport.Width, Scene.ActiveScene.Device.Viewport.Height)*0.5f)*Transform.METRES_A_PIXEL)/Camera.GameObject.Transform.Scale + Camera.GameObject.Transform.Position;
        }
    }
}
