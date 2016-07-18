using System.Collections.Generic;
using System.IO;
using Editor.Actions;
using Editor.Scripts;
using Editor.Scripts.Placer;
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
    public class EditorGame : Scene
    {
        private GameObject camera;
        private GameObject cursor;
        private MapInfo map;

        private ActionManager actionManager;
        private BlockInfo blockInfo = new BlockInfo();
        private Placer placer;

        public EditorGame()
        {
        }

        public override void Start()
        {
            map = new MapInfo();

            base.Start();

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);

            cursor = GameObjectFactory.New().With(new MouseComponent()).Create();
            PrefabFactory.Instantiate(cursor);

            camera = CameraBuilder.CreateCamera(cursor.Transform);
            PrefabFactory.Instantiate(camera);
            
            var scene = GameObjectFactory.New().Create();
            PrefabFactory.Instantiate(scene);

            map.Scene = new PrefabInfo() {PrefabName = "empty", Children = new List<PrefabInfo>()};

            //Instantiate the action manager so we can undo/redo
            actionManager = new ActionManager(map, scene, PrefabFactory);

            //Create placer
            placer = new Placer();
            placer.Manager = actionManager;
            var placerObj = GameObjectFactory.New()
                .With(placer)
                .Create();

            PrefabFactory.Instantiate(placerObj);

            CreateShadowPrefabs();
            SetPlacing("platform");
        }

        private void SetPlacing(string platform)
        {
            placer.Settings.Shadow = blockInfo.GetShadow(platform);
            placer.Settings.NoSize = !blockInfo.IsSizable(platform);
            placer.Settings.PlacingPrefab = platform;
        }

        private void CreateShadowPrefabs()
        {
            var platformShadow = GameObjectFactory
                .New()
                .With(new Sprite()
                {
                    Texture = TextureUtil.CreateTexture(64, 64, Color.Black),
                    Tint = Color.Lerp(Color.White, Color.Transparent, 0.2f)
                })
                .Create();
            platformShadow.Visible = false;
            PrefabFactory.Instantiate(platformShadow);
            blockInfo.AddInfo("platform", platformShadow, true);
        }

        public override void Update(float step)
        {
            if (Input.IsPressed(Keys.Z) && actionManager.CanUndo)
                actionManager.Undo();
            if (Input.IsPressed(Keys.Y) && actionManager.CanRedo)
                actionManager.Redo();

            base.Update(step);
        }
    }
}
