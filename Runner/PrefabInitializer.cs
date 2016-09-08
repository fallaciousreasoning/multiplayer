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
        public static void AddRunnerGamePrefabs(PrefabManager prefabManager)
        {
            prefabManager.RegisterPrefab("platform", ObstacleBuilder.Obstacle);
            prefabManager.RegisterPrefab("player", PlayerBuilder.Player);
        }
    }
}
