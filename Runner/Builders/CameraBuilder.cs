using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;
using Runner.Components;

namespace Runner.Builders
{
    public static class CameraBuilder
    {
        public static EntityBuilder Camera(Transform target)
        {
            return EntityBuilder.New()
                .With(new Camera())
                .With(new Follow()
                {
                    Target = target
                });
        }
    }
}
