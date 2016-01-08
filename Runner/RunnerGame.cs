using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
using Runner.Builders;

namespace Runner
{
    public class RunnerGame : Game1
    {
        protected override void Initialize()
        {
            base.Initialize();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            PrefabFactory.RegisterPrefab("ground", () => ObstacleBuilder.BuildGround(12.5f).Create());
            PrefabFactory.RegisterPrefab("player", () => PlayerBuilder.BuildPlayer().Create());

            PrefabFactory.Instantiate("player", new Vector2(6.25f, 0));
            PrefabFactory.Instantiate("ground", new Vector2(6.25f, 7));
        }
    }
}
