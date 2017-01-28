using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Factories;
using Runner.Components;

namespace Runner.Prefabs
{
    public class BuildingPrefab : IPrefab
    {
        private const int SEED = 17560832;
        private const int BUILDINGS = 5;

        public Entity Build()
        {
            var random = new Random(SEED);

            var builder = EntityBuilder.New();

            for (var i = 0; i < BUILDINGS; ++i)
            {
                var building = EntityBuilder.New()
                    .With(new BuildingInfo()
                    {
                        HeightInTiles = random.Next(10, 50),
                        WidthInTiles = random.Next(5, 8),
                        Z = (float)random.NextDouble() * 30,
                        TopLeftCorner = TextureUtil.CreateTexture(64, 64, Color.DarkGray),
                        TopTexture = TextureUtil.CreateTexture(64, 64, Color.DarkGray),
                        WallTexture = TextureUtil.CreateTexture(64, 64, Color.LightGray),
                        InnerTexture = TextureUtil.CreateTexture(64, 64, Color.LightGray)
                    })
                    .AtPosition(new Vector2((float)random.NextDouble() * 20 - 10, 0));
                builder.WithChild(building);
            }

            return builder.Create();
        }
    }
}
