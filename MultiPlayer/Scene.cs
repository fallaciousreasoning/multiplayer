using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core;
using MultiPlayer.Core.InputMethods;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Physics;

namespace MultiPlayer
{
    public class Scene : ComponentManager
    {
        private static SpriteBatch spriteBatch;

        public PhysicsWorld PhysicsWorld { get; private set; }
        public SpriteFont DefaultFont { get; private set; }

        public PrefabFactory PrefabFactory { get; private set; }
        public InputManager Input { get; private set; }
        
        public SpriteBatch SpriteBatch => spriteBatch;
        public GraphicsDevice Device { get; set; }

        public ComponentManager ComponentManager;
        public ComponentManager<GameObject> GameObjectManager;

        public static Scene ActiveScene { get; private set; }
        
        public Scene(IMouse mouse, IKeyboard keyboard)
        {
            Input = new InputManager(mouse, keyboard);
            ActiveScene = this;
        }

        public override void Start() 
        {
            spriteBatch = new SpriteBatch(Device);

            ComponentManager = new ComponentManager();
            ComponentManager.DelayedAdd(new Logger());

            PhysicsWorld = new PhysicsWorld();
            ComponentManager.DelayedAdd(PhysicsWorld);

            GameObjectManager = new ComponentManager<GameObject>();
            ComponentManager.DelayedAdd(GameObjectManager);

            PrefabFactory = new PrefabFactory(GameObjectManager);
            ComponentManager.DelayedAdd(PrefabFactory);

            Input.AddButton("shoot", new InputButton(Keys.Space));
            ComponentManager.DelayedAdd(Input);

            DelayedAdd(ComponentManager);

            base.Start();
        }

        public override void Draw()
        {
            Device.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(0, null, null, null, null, null, Camera.ActiveCamera?.World ?? Matrix.Identity);

            base.Draw();

            SpriteBatch.End();
        }
    }
}
