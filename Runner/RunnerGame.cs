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
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            PrefabFactory.RegisterPrefab("ground", () => ObstacleBuilder.BuildGround().Create());
            PrefabFactory.RegisterPrefab("player", () => PlayerBuilder.BuildPlayer().Create());
            PrefabFactory.RegisterPrefab("animated", PlayerBuilder.AnimationTest);

            PrefabFactory.Instantiate("player", new Vector2(6.25f, 0));
            PrefabFactory.Instantiate("ground", new Vector2(6.5f, 7), 0, new Vector2(13, 1));
            PrefabFactory.Instantiate("ground", new Vector2(0.5f, 4), 0, new Vector2(1, 8));
            PrefabFactory.Instantiate("ground", new Vector2(12f, 4), 0, new Vector2(1, 8));

            PrefabFactory.Instantiate("ground", new Vector2(11, 5.5f), 0, new Vector2(2, 5));

            //PrefabFactory.Instantiate("animated");
        }
    }
}
