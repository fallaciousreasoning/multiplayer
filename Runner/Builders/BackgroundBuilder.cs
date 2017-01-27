using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Factories;

namespace Runner.Builders
{
    public static class BackgroundBuilder
    {
        private const int SEED = 17560832;
        private const int BUILDINGS = 10;

        public static EntityBuilder Build()
        {
            var random = new Random(SEED);

            var builder = new EntityBuilder();
            var children = new List<EntityBuilder>();

            return builder;
        }
    }
}
