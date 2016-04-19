using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class Follow : IUpdateable, IKnowsGameObject
    {
        public void Update(float step)
        {
            var newPos = Vector2.Lerp(GameObject.Transform.Position, Target.Position, Spring);
            GameObject.Transform.Position = newPos;
        }

        public float Spring { get; set; } = 0.2f;
        public Transform Target { get; set; }
        public GameObject GameObject { get; set; }
    }
}
