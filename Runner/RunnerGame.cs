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
using MultiPlayer.Factories;
using MultiPlayer.Serialization;
using Newtonsoft.Json;
using Runner.Builders;
using Runner.Components;
using Runner.Prefabs;
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

            Engine.AddSystem(new BuildingRenderer());

            Engine.AddSystem(new CharacterSystem());
            Engine.AddSystem(new Mover());
            Engine.AddSystem(new Roller());
            Engine.AddSystem(new Clamberer());
            Engine.AddSystem(new Slider());
            Engine.AddSystem(new Diver());

            Engine.AddSystem(new PlayerInput());

            Engine.AddSystem(new DiveInitiator());

            base.Start();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));

            PrefabInitializer.AddRunnerGamePrefabs(PrefabManager);

            var player = PrefabManager.Instantiate("player", new Vector2(2, -.25f));

            PrefabManager.Instantiate("platform", new Vector2(0, 0), 0, new Vector2(48, 1));
            PrefabManager.Instantiate("platform", new Vector2(-23.5f, -10), 0, new Vector2(1, 20));
            PrefabManager.Instantiate("platform", new Vector2(23.5f, -10), 0, new Vector2(1, 20));
            PrefabManager.Instantiate("platform", new Vector2(21f, -2.5f), 0, new Vector2(4, 4));

            PrefabManager.Instantiate("platform", new Vector2(-10, -3f), 0, new Vector2(4, 4));

            PrefabManager.Instantiate("divable", new Vector2(0f, -1.5f), 0, new Vector2(1, 1));
            PrefabManager.Instantiate("buildings", new Vector2(0, 15));

            camera = new CameraPrefab(player.Get<Transform>()).Build();

            var json = new EntitySerializer().Serialize(camera);
            File.WriteAllText("entity.json", json);
            var result = new EntitySerializer().Deserialize(json);
            Engine.AddEntity(camera);
        }

        public RunnerGame(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
        }
    }
}
