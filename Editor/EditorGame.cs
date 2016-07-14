using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core;
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

        protected override void Initialize()
        {
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

            PrefabFactory.Instantiate("ground", new Vector2(6.5f, 7), 0, new Vector2(13, 1));
            PrefabFactory.Instantiate("ground", new Vector2(0.5f, 4), 0, new Vector2(1, 8));
            PrefabFactory.Instantiate("ground", new Vector2(12f, 4), 0, new Vector2(1, 8));

            PrefabFactory.Instantiate("ground", new Vector2(11, 5.5f), 0, new Vector2(2, 5));
            PrefabFactory.Instantiate("ground", new Vector2(1, 5.5f), 0, new Vector2(2, 5));
        }
    }
}
