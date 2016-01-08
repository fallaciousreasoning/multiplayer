using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer;
using MultiPlayer.GameComponents.Physics;

namespace Runner.Builders
{
    public class TriggerDetector : IHearsTrigger
    {
        public string TriggeredBy = "Ground";

        public bool OnGround => touchingTriggers.Count > 0;

        private readonly List<GameObject> touchingTriggers = new List<GameObject>();

        public void OnTriggerEntered(GameObject hit)
        {
            if (hit.Tag.Name == TriggeredBy && !touchingTriggers.Contains(hit))
                touchingTriggers.Add(hit);
        }

        public void OnTriggerExited(GameObject separated)
        {
            if (touchingTriggers.Contains(separated)) touchingTriggers.Remove(separated);
        }
    }
}
