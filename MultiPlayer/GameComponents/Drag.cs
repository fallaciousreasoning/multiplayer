using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class Drag : IUpdateable, IKnowsGameObject, IStartable
    {
        private VelocityController velocityController;

        public float FrictionCoefficient = 2;
        public float AngularFrictionCoefficient = 5;
         
        public void Update(float step)
        {
            velocityController.Velocity -= velocityController.Velocity*FrictionCoefficient*step;
            velocityController.AngularVelocity -= velocityController.AngularVelocity*AngularFrictionCoefficient*step;
        }

        public GameObject GameObject { get; set; }

        public void Start()
        {
            velocityController = GameObject.GetComponent<VelocityController>();
        }
    }
}
