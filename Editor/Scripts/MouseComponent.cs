using MultiPlayer;
using MultiPlayer.GameComponents;

namespace Editor.Scripts
{
    public class MouseComponent : IUpdateable, IKnowsGameObject
    {
        public void Update(float step)
        {
            if (Game1.Game.Input.IsDown(MouseButton.Right))
                GameObject.Transform.Position -= Game1.Game.Input.MouseMoved*Transform.METRES_A_PIXEL/Camera.ActiveCamera.GameObject.Transform.Scale;
        }

        public GameObject GameObject { get; set; }
    }
}
