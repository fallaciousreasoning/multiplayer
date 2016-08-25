using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents
{
    public class Weapon : IUpdateable, IStartable, IKnowsGameObject
    {
        public float FireRate = 1;

        public string BulletPrefab = "bullet";

        private float tillFire;

        public void Update(float step)
        {
            tillFire -= step;
            if (Scene.ActiveScene.Input.GetButtonDown("shoot") && tillFire < 0)
                Shoot();
        }

        private void Shoot()
        {
            var bullet = Scene.ActiveScene.PrefabFactory.Instantiate(BulletPrefab, GameObject.Transform.Position, GameObject.Transform.Rotation);
            tillFire = FireRate;
        }

        public void Start()
        {
            tillFire = FireRate;
        }

        public GameObject GameObject { get; set; }
    }
}
