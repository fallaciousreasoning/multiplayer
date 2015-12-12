using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class ShipEngine : IStartable, IKnowsGameObject
    {
        public float Acceleration = 120f;

        private VelocityController velocityController;

        public void Start()
        {
            velocityController = GameObject.GetComponent<VelocityController>();
        }

        public GameObject GameObject { get; set; }

        public void ApplyThrust(float step)
        {
            var dir = GameObject.Transform.FacingDirection;
            velocityController.Velocity += dir*Acceleration*step;
        }
    }
}
