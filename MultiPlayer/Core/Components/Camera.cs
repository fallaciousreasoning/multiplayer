using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Components
{
    public class Camera
    {
        public static Camera ActiveCamera { get; set; }

        public Camera()
        {
            if (ActiveCamera == null)
                ActiveCamera = this;
        }
        
        public Matrix World { get; set; } = Matrix.Identity;
    }
}
