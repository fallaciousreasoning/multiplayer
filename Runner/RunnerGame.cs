using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.InputMethods;
using Newtonsoft.Json;
using Runner.Builders;

namespace Runner
{
    public class RunnerGame : Scene
    {
        private Entity camera;

        public override void Start()
        {
            base.Start();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));

            PrefabInitializer.AddRunnerGamePrefabs(PrefabManager);

            var player = PrefabManager.Instantiate("player", new Vector2(-6.25f, 0));

            camera = CameraBuilder.Camera(player.Transform).CreateLast();
            Engine.AddEntity(camera);
        }

        public RunnerGame(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
        }
    }
}
