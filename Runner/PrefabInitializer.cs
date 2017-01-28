using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using Runner.Builders;
using Runner.Prefabs;

namespace Runner
{
    public static class PrefabInitializer
    {
        public static void AddRunnerGamePrefabs(PrefabManager prefabManager)
        {
            prefabManager.RegisterPrefab("platform", new PlatformPrefab());
            prefabManager.RegisterPrefab("divable", new DivablePrefab());

            prefabManager.RegisterPrefab("player", new CharacterPrefab());
            prefabManager.RegisterPrefab("buildings", new BuildingPrefab());
        }
    }
}
