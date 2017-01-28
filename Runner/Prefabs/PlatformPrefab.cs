using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;
using Runner.Components;

namespace Runner.Prefabs
{
    public class PlatformPrefab : IPrefab
    {
        public Entity Build()
        {
            int width = 1;
            int height = 1;
            return EntityBuilder.New()
                .WithTag(Tags.PLATFORM)
                .WithTexture(TextureUtil.CreateTexture((int)(width * Transform.PIXELS_A_METRE), (int)(height * Transform.PIXELS_A_METRE), Color.Black))
                .With(ColliderBuilder.New().BoxShape(width, height).Create())
                .Create();
        }
    }
}
