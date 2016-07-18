using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class Camera : IUpdateable, IKnowsGameObject
    {
        public static Camera ActiveCamera { get; set; }

        public Camera()
        {
            if (ActiveCamera == null)
                ActiveCamera = this;
        }

        public Matrix World { get; private set; } = Matrix.Identity;

        public void Update(float step)
        {
            World = Matrix.CreateRotationZ(GameObject.Transform.Rotation)*
                    Matrix.CreateTranslation(new Vector3(-GameObject.Transform.Position*Transform.PIXELS_A_METRE, 0))*
                    Matrix.CreateScale(new Vector3(GameObject.Transform.Scale, 0))*
                    Matrix.CreateTranslation(new Vector3(Scene.ActiveScene.Device.Viewport.Width, Scene.ActiveScene.Device.Viewport.Height, 0) * 0.5f);
        }

        public GameObject GameObject { get; set; }
    }
}
