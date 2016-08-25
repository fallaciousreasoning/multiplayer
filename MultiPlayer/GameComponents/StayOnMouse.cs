using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class StayOnMouse : IUpdateable, IKnowsGameObject
    {
        public void Update(float step)
        {
            GameObject.Transform.Position = Scene.ActiveScene.Input.MousePosition*Transform.METRES_A_PIXEL;
        }

        public GameObject GameObject { get; set; }
    }
}
