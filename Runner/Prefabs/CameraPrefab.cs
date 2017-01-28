using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;
using Runner.Components;

namespace Runner.Prefabs
{
    public class CameraPrefab : IPrefab
    {
        private Transform target;
        public CameraPrefab(Transform target)
        {
            this.target = target;
        }

        public Entity Build()
        {
            return EntityBuilder.New()
                .With(new Camera())
                .With(new Follow()
                {
                    Spring = 5,
                    Target = target
                })
                .Create();
        }
    }
}
