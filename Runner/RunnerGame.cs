using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
using Newtonsoft.Json;
using Runner.Builders;

namespace Runner
{
    public class RunnerGame : Game1
    {
        private GameObject camera;
        protected override void Initialize()
        {
            base.Initialize();

            Input.AddButton("jump", new InputButton(Keys.Space, Keys.W, Keys.Up));
            Input.AddButton("slide", new InputButton(Keys.S, Keys.LeftShift, Keys.Down));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);
            
            var player = PrefabFactory.Instantiate("player", new Vector2(6.25f, 0));

            camera = CameraBuilder.CreateCamera(player.Transform);

            PrefabFactory.Instantiate(camera);

            string json;
            using (var r = new StreamReader(File.OpenRead("map.json")))
            {
                json = r.ReadToEnd();
            }
            var map = JsonConvert.DeserializeObject<MapInfo>(json);
            PrefabFactory.Instantiate(map.Scene);

            //PrefabFactory.Instantiate("ground", new Vector2(6.5f, 7), 0, new Vector2(13, 1));
            //PrefabFactory.Instantiate("ground", new Vector2(0.5f, 4), 0, new Vector2(1, 8));
            //PrefabFactory.Instantiate("ground", new Vector2(12f, 4), 0, new Vector2(1, 8));

            //PrefabFactory.Instantiate("ground", new Vector2(11, 5.5f), 0, new Vector2(2, 5));
            //PrefabFactory.Instantiate("ground", new Vector2(1, 5.5f), 0, new Vector2(2, 5));
        }
    }
}
