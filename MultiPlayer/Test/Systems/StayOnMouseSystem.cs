using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Systems;
using MultiPlayer.Test.Families;

namespace MultiPlayer.Test
{
    public class StayOnMouseSystem : UpdatableSystem
    {
        public StayOnMouseSystem() : base(new [] {typeof(StayOnMouseFamily)})
        {
        }

        protected override void Update(Time time)
        {
            var input = Engine.Scene.Input;
            var family = Engine.FamilyManager.Get<StayOnMouseFamily>();

            foreach (var entity in family.Entities)
            {
                entity.Get<Transform>().Position = input.MousePosition;
            }
        }
    }
}
