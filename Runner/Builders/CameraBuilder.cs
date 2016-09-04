using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer;
using MultiPlayer.Core.Components;
using Runner.Components;

namespace Runner.Builders
{
    public static class CameraBuilder
    {
        public static GameObject CreateCamera(Transform target)
        {
            return GameObjectFactory.New()
                .With(new Camera())
                .With(new Follow()
                {
                    Target = target
                })
                .Create();
        }
    }
}
