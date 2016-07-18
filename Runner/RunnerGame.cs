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
using Newtonsoft.Json;
using Runner.Builders;

namespace Runner
{
    public class RunnerGame : Scene
    {
        private GameObject camera;

        public RunnerGame()
        {
        }

        public override void Start()
        {
            base.Start();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);

            var player = PrefabFactory.Instantiate("player", new Vector2(-6.25f, 0));

            camera = CameraBuilder.CreateCamera(player.Transform);

            PrefabFactory.Instantiate(camera);

            string json;
            using (var r = new StreamReader(File.OpenRead("map.json")))
            {
                json = r.ReadToEnd();
            }
            var map = JsonConvert.DeserializeObject<MapInfo>(json);
            PrefabFactory.Instantiate(map.Scene);
        }
    }
}
