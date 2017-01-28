using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;
using Runner.Builders;
using Runner.Components;

namespace Runner.Prefabs
{
    public class DivablePrefab : IPrefab
    {
        public Entity Build()
        {
            int width = 4;
            int height = 3;

            float detectorYOffset = 0.5f;

            float detectorOverhang = 1f;
            float detectorHeight = 1f;

            float detectorWidth = width + detectorOverhang * 2;

            var characterDetector = new Touching();

            return EntityBuilder.New()
                .WithTag(Tags.PLATFORM)
                .WithTexture(TextureUtil.CreateTexture((int)(width * Transform.PIXELS_A_METRE),
                    (int)(height * Transform.PIXELS_A_METRE), Color.Black))
                .With(ColliderBuilder.New().BoxShape(width, height).Create())
                .With(new Divable()
                {
                    Touching = characterDetector
                })
                .WithChild(EntityBuilder.New()
                    .With(characterDetector)
                    .AtPosition(new Vector2(0, -height * 0.5f - detectorYOffset - detectorHeight * 0.5f))
                    //.WithTexture(TextureUtil.CreateTexture((int) (detectorWidth*Transform.PIXELS_A_METRE),
                    //    (int) (detectorHeight*Transform.PIXELS_A_METRE), Color.Red))
                    .With(ColliderBuilder.New().BoxShape(detectorWidth, detectorHeight).IsTrigger().Create())
                )
                .Create();
        }
    }
}
