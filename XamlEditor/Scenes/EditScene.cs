using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core.InputMethods;
using Runner;

namespace XamlEditor.Scenes
{
    public class EditScene : Scene
    {
        public GameObject EditRoot { get; private set; }
        public GameObject SceneRoot { get; private set; }

        public EditScene(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
        }

        public override void Start()
        {
            base.Start();

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);

            EditRoot = GameObjectFactory.New().Create();
            SceneRoot = GameObjectFactory.New().Create();

            Add(EditRoot);
            Add(SceneRoot);

            var platform = PrefabFactory.Instantiate("platform");
            platform.Transform.Position = new Vector2();
            platform.Transform.Scale = new Vector2(5, 3);

            SceneRoot.DelayedAdd(platform);
        }
    }
}
