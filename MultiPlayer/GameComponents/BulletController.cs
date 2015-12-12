using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents
{
    public class BulletController : IStartable, IKnowsGameObject, IUpdateable
    {
        public float LifeTime = 2;
        public float Speed = 5;
        
        public Vector2 StartingPosition;

        public void Start()
        {
            StartingPosition = GameObject.Transform.LocalPosition;

            var dir = GameObject.Transform.FacingDirection;
            GameObject.GetComponent<VelocityController>().Velocity = dir*Speed;
        }

        public void Update(float step)
        {
            LifeTime -= step;

            if (LifeTime <= 0)
                GameObject.ShouldRemove = true;
        }

        public GameObject GameObject { get; set; }
    }
}
