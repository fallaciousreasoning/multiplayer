﻿using MultiPlayer.Core;
using MultiPlayer.Core.Systems;
using MultiPlayer.Test.Families;

namespace MultiPlayer.Test.Systems
{
    public class StayOnMouseSystem : UpdatableSystem<StayOnMouseFamily>
    {
        protected override void Update(Time time)
        {
            var input = Engine.Scene.Input;
            var family = Engine.FamilyManager.GetNodeFamily<StayOnMouseFamily>();

            foreach (var node in family.Nodes)
            {
                node.Transform.Position = input.MousePosition;
            }
        }
    }
}
