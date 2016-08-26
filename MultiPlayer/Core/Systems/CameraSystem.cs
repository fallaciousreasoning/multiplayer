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
    public class CameraSystem : UpdatableSystem
    {
        public Camera ActiveCamera { get; set; } = new Camera();

        public CameraSystem()
            : base(new []{typeof(CameraFamily)})
        {
        }

        protected override void Update(Time time)
        {
            var nodes = Engine.FamilyManager.Get<CameraFamily>().Entities;
            foreach (var node in nodes)
            {
                var transform = node.Get<Transform>();
                var camera = node.Get<Camera>();

                camera.World = Matrix.CreateRotationZ(transform.Rotation) *
                    Matrix.CreateTranslation(new Vector3(-transform.Position * Transform.PIXELS_A_METRE, 0)) *
                    Matrix.CreateScale(new Vector3(transform.Scale, 0)) *
                    Matrix.CreateTranslation(new Vector3(Scene.ActiveScene.Device.Viewport.Width, Scene.ActiveScene.Device.Viewport.Height, 0) * 0.5f);
            }
        }
    }
}
