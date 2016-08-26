using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Nodes;

namespace MultiPlayer.Core.Systems
{
    public class CameraSystem : UpdatableSystem
    {
        public static CameraSystem Active { get; private set; }

        public Matrix CameraWorld => ActiveCamera?.Get<Camera>()?.World ?? Matrix.Identity;

        public Entity ActiveCamera { get; set; }

        public float ScreenWidth => Scene.ActiveScene.Device.Viewport.Width*Transform.METRES_A_PIXEL;
        public float ScreenHeight => Scene.ActiveScene.Device.Viewport.Height*Transform.METRES_A_PIXEL;

        public CameraSystem()
            : base(new []{typeof(CameraNode)})
        {
            Active = this;
        }

        protected override void Update(Time time)
        {
            var nodes = Engine.FamilyManager.Get<CameraNode>().Nodes;
            foreach (var node in nodes)
            {
                var transform = node.Transform;
                var camera = node.Camera;

                camera.World = Matrix.CreateRotationZ(transform.Rotation) *
                    Matrix.CreateTranslation(new Vector3(-transform.Position * Transform.PIXELS_A_METRE, 0)) *
                    Matrix.CreateScale(new Vector3(transform.Scale, 0)) *
                    Matrix.CreateTranslation(new Vector3(Scene.ActiveScene.Device.Viewport.Width, Scene.ActiveScene.Device.Viewport.Height, 0) * 0.5f);
            }
        }

        public Vector2 ScreenToWorld(Vector2 screen)
        {
            var camera = ActiveCamera.Get<Camera>();
            var transform = ActiveCamera.Get<Transform>();

            screen *= Transform.METRES_A_PIXEL;
            screen -= new Vector2(ScreenWidth, ScreenHeight) * 0.5f;
            screen /= transform.Scale;
            screen += transform.Position;

            return screen;
        }

        public Vector2 WorldToScreen(Vector2 world)
        {
            var camera = ActiveCamera.Get<Camera>();
            var transform = ActiveCamera.Get<Transform>();

            world -= transform.Position;
            world *= transform.Scale;
            world += new Vector2(ScreenWidth, ScreenHeight) * 0.5f;
            world *= Transform.PIXELS_A_METRE;

            return world;
        }
    }
}
