using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;

namespace Runner.Builders
{
    public static class ObstacleBuilder
    {
        public const string OBSTACLE_TAG = "obstacle";

        public static EntityBuilder Obstacle()
        {
            int width = 1;
            int height = 1;
            return EntityBuilder.New()
                .WithTag(OBSTACLE_TAG)
                .WithTexture(TextureUtil.CreateTexture((int)(width*Transform.PIXELS_A_METRE), (int)(height*Transform.PIXELS_A_METRE), Color.Black))
                .With(ColliderBuilder.New().BoxShape(width, height).Create());
        }
    }
}
