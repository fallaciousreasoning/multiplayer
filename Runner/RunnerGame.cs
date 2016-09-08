using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.InputMethods;
using Newtonsoft.Json;
using Runner.Builders;
using Runner.Systems;

namespace Runner
{
    public class RunnerGame : Scene
    {
        private Entity camera;

        public override void Start()
        {
            Engine.AddSystem(new FollowSystem());

            Engine.AddSystem(new TouchTrackingSystem());

            Engine.AddSystem(new CharacterSystem());
            Engine.AddSystem(new Mover());
            Engine.AddSystem(new Roller());
            Engine.AddSystem(new Clamberer());

            Engine.AddSystem(new PlayerInput());

            base.Start();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));

            PrefabInitializer.AddRunnerGamePrefabs(PrefabManager);

            var player = PrefabManager.Instantiate("player", new Vector2(2, -.25f));

            PrefabManager.Instantiate("platform", new Vector2(0, 3), 0, new Vector2(12, 1));
            PrefabManager.Instantiate("platform", new Vector2(-6.5f, 0), 0, new Vector2(1, 6));
            PrefabManager.Instantiate("platform", new Vector2(6.5f, 0), 0, new Vector2(1, 6));
            PrefabManager.Instantiate("platform", new Vector2(5.5f, 1), 0, new Vector2(1, 4));

            camera = CameraBuilder.Camera(player.Last().Get<Transform>()).CreateRoot();
            Engine.AddEntity(camera);
        }

        public RunnerGame(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
        }
    }
}
