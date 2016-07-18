using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;

namespace Editor.Scripts
{
    public class MouseComponent : IUpdateable, IKnowsGameObject
    {
        public void Update(float step)
        {
            if (Scene.ActiveScene.Input.IsDown(MouseButton.Right))
                GameObject.Transform.Position -= Scene.ActiveScene.Input.MouseMoved*Transform.METRES_A_PIXEL/Camera.ActiveCamera.GameObject.Transform.Scale;
        }

        public GameObject GameObject { get; set; }
    }
}
