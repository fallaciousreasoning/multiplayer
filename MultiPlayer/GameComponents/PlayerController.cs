using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class PlayerController : IKnowsGameObject, IStartable, IUpdateable
    {
        public GameObject GameObject { get; set; }

        public float AngularAcceleration = 100;

        private VelocityController velocityController;
        private ShipEngine engine;

        public void Update(float step)
        {
            if (Scene.ActiveScene.Input.GetButtonDown("shoot"))
            {
                //TODO shoot
            }

            var horizontal = Scene.ActiveScene.Input.GetAxis("horizontal");
            velocityController.AngularVelocity -= horizontal*AngularAcceleration*step;

            if (Scene.ActiveScene.Input.GetAxis("vertical") < 0)
                engine.ApplyThrust(step);
        }

        public void Start()
        {
            velocityController = GameObject.GetComponent<VelocityController>();
            engine = GameObject.GetComponent<ShipEngine>();
        }
    }
}
