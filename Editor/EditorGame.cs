using System.Collections.Generic;
using System.IO;
using Editor.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;
using Newtonsoft.Json;
using Runner;
using Runner.Builders;

namespace Editor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class EditorGame : Game1
    {
        private GameObject camera;
        private GameObject cursor;
        private MapInfo map;

        protected override void Initialize()
        {
            map = new MapInfo();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);

            cursor = GameObjectFactory.New().With(new MouseComponent()).Create();
            PrefabFactory.Instantiate(cursor);

            camera = CameraBuilder.CreateCamera(cursor.Transform);
            PrefabFactory.Instantiate(camera);
            
            var scene = GameObjectFactory.New().Create();
            PrefabFactory.Instantiate(scene);

            map.Scene = new PrefabInfo() {PrefabName = "empty", Children = new List<PrefabInfo>()};

            //Create placer
            var placer = new Placer()
            {
                Place = (topLeft, size) =>
                {
                    var pos = topLeft + size*0.5f;
                    PrefabFactory.Instantiate("ground",pos , 0, size, scene);
                    map.Scene.Children.Add(new PrefabInfo()
                    {
                        PrefabName = "ground",
                        Position = pos,
                        Scale = size,
                    });
                    var json = JsonConvert.SerializeObject(map);
                    using (var w = new StreamWriter(File.Create("map.json")))
                    {
                        w.Write(json);
                    }
                }
            };
            var renderer = new ShadowRenderer()
            {
                Placer = placer
            };
            var placerObj = GameObjectFactory.New()
                .With(placer)
                .With(renderer)
                .Create();

            PrefabFactory.Instantiate(placerObj);
        }
    }
}
