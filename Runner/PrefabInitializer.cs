using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using Runner.Builders;

namespace Runner
{
    public static class PrefabInitializer
    {
        public static void AddRunnerGamePrefabs(PrefabFactory factory)
        {
            factory.RegisterPrefab("ground", () => ObstacleBuilder.BuildGround().Create());
            factory.RegisterPrefab("player", () => PlayerBuilder.BuildPlayer().Create());
            factory.RegisterPrefab("animated", PlayerBuilder.AnimationTest);
        }
    }
}
