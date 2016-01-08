using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer;
using MultiPlayer.GameComponents.Physics;

namespace Runner.Builders
{
    public class GroundDetector : IHearsTrigger
    {
        public string GroundTag = "Ground";

        public bool OnGround => touchingGroundObjects.Count > 0;

        private readonly List<GameObject> touchingGroundObjects = new List<GameObject>();

        public void OnTriggerEntered(GameObject hit)
        {
            if (hit.Tag.Name == GroundTag && !touchingGroundObjects.Contains(hit))
                touchingGroundObjects.Add(hit);
        }

        public void OnTriggerExited(GameObject separated)
        {
            if (touchingGroundObjects.Contains(separated)) touchingGroundObjects.Remove(separated);
        }
    }
}
