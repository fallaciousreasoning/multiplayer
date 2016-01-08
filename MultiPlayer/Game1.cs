using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Physics;

namespace MultiPlayer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        public PhysicsWorld PhysicsWorld { get; private set; }
        public SpriteFont DefaultFont { get; private set; }
        public PrefabFactory PrefabFactory { get; private set; }
        public InputManager Input { get; private set; }
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public GraphicsDevice Device { get { return graphics.GraphicsDevice; } }

        public ComponentManager ComponentManager;
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

            PhysicsWorld = new PhysicsWorld();
            ComponentManager.DelayedAdd(PhysicsWorld);

            GameObjectManager = new ComponentManager<GameObject>();
            ComponentManager.DelayedAdd(GameObjectManager);

            PrefabFactory = new PrefabFactory(GameObjectManager);
            ComponentManager.DelayedAdd(PrefabFactory);

            ComponentManager.Start();

            Input = new InputManager(this);
            Components.Add(Input);
            Input.AddButton("shoot", new InputButton(Keys.Space));

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

            //PrefabFactory.RegisterPrefab("machine gun", () => GameObjectFactory.New()
            //    .WithTexture(TextureUtil.CreateTexture(16, 16, Color.Red))
            //    .With(new Weapon() { FireRate = 0.5f })
            //    .Create());

            //PrefabFactory.RegisterPrefab("player", () => GameObjectFactory.New()
            //    .AtPosition(new Vector2(0))
            //    .WithTexture(TextureUtil.CreateTexture(64, 128, Color.Black))
            //    .With(new VelocityController())
            //    .With(new ShipEngine())
            //    .With(new PlayerController())
            //    .With(new Drag())
            //    .With(new ScreenWrapper()).With(new ParticleEmitter())
            //    .WithChild("machine gun", new Vector2(0, -1))
            //    .Create());

            //PrefabFactory.RegisterPrefab("bullet", () => GameObjectFactory.New()
            //    .WithTexture(TextureUtil.CreateTexture(16, 16, Color.Yellow))
            //    .With(new VelocityController())
            //    .With(new BulletController())
            //    .With(new ScreenWrapper())
            //    .Create());

            PrefabFactory.RegisterPrefab("unitbox", () => GameObjectFactory.New()
                .WithTexture(TextureUtil.CreateTexture(64, 64, Color.Black))
                .With(new Collider()
                {
                    Body = BodyFactory.CreateRectangle(PhysicsWorld.World, 1, 1, 1)
                })
                .Create());

            PrefabFactory.RegisterPrefab("cursor", () => GameObjectFactory.New()
                .WithTexture(TextureUtil.CreateTexture(64, 64, Color.Red))
                .With(new SimpleController()).With(new Collider()
                {
                    Body = BodyFactory.CreateRectangle(PhysicsWorld.World, 1, 1, 1),
                    BodyType = BodyType.Dynamic
                })
                .Create());

            GameObjectManager.Start();
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
