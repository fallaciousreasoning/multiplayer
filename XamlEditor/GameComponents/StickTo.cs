using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.GameComponents;

namespace XamlEditor.GameComponents
{
    public class StickTo : IKnowsGameObject, ILateUpdateable
    {
        public GameObject To { get; set; }
        public GameObject GameObject { get; set; }

        public void LateUpdate(float step)
        {
            GameObject.Transform.Set(To.Transform);
        }
    }
}
