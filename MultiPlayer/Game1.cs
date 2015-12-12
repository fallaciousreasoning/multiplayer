using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;

namespace MultiPlayer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        public SpriteFont DefaultFont { get; private set; }
        public PrefabFactory PrefabFactory { get; private set; }
        public InputManager Input { get; private set; }
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public GraphicsDevice Device { get { return graphics.GraphicsDevice; } }

        private ComponentManager ComponentManager;
        public ComponentManager<GameObject> GameObjectManager;

        public static Game1 Game { get; private set; }

        public Game1()
        {
            if (Game != null) throw new Exception("There can only be one game!");

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Game = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ComponentManager = new ComponentManager();
            ComponentManager.DelayedAdd(new Logger());

            ComponentManager.Start();

            GameObjectManager = new ComponentManager<GameObject>();
            ComponentManager.DelayedAdd(GameObjectManager);

            Input = new InputManager(this);
            Components.Add(Input);
            Input.AddButton("shoot", new InputButton(Keys.Space));

            PrefabFactory = new PrefabFactory(GameObjectManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            DefaultFont = Content.Load<SpriteFont>("Default");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            PrefabFactory.RegisterPrefab("machine gun", (p) => GameObjectFactory.New()
                .AtPosition(p)
                .WithTexture(TextureUtil.CreateTexture(16, 16, Color.Red))
                .With(new Weapon() { FireRate = 0.5f })
                .Create());

            PrefabFactory.RegisterPrefab("player", () => GameObjectFactory.New()
                .AtPosition(new Vector2(0))
                .WithTexture(TextureUtil.CreateTexture(64, 128, Color.Black))
                .With(new VelocityController())
                .With(new ShipEngine())
                .With(new PlayerController())
                .With(new Drag())
                .With(new ScreenWrapper())
                .WithChild("machine gun", new Vector2(0, -1))
                .Create());

            PrefabFactory.RegisterPrefab("bullet", (v, f) => GameObjectFactory.New()
                .AtPosition(v)
                .WithRotation(f)
                .WithTexture(TextureUtil.CreateTexture(16, 16, Color.Yellow))
                .With(new VelocityController())
                .With(new BulletController())
                .With(new ScreenWrapper())
                .Create());

            GameObjectManager.Start();

            var player = PrefabFactory.Instantiate("player");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ComponentManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            ComponentManager.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
