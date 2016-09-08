using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation;
using MultiPlayer.Core.Animation.Messages;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.Systems;
using MultiPlayer.Factories;
using MultiPlayer.Test;
using MultiPlayer.Test.Components;
using MultiPlayer.Test.Systems;

namespace MultiPlayer
{
    public class EnigmaGame : Game
    {
        private static GraphicsDeviceManager graphics;
        private readonly Scene scene;
        private readonly Time time;

        public EnigmaGame(Scene scene)
        {
            this.scene = scene;
            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            time = new Time();
        }

        protected override void Initialize()
        {
            scene.Device = graphics.GraphicsDevice;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            scene.Start();

            return;

            scene.Engine.AddSystem(new StayOnMouseSystem());
            scene.Engine.AddSystem(new TestCollisionListener());

            var cursor = new Entity();
            cursor.Add(new Transform());
            cursor.Add(new Sprite()
            {
                Texture = TextureUtil.CreateTexture(32, 32, Color.Black)
            });
            cursor.Add(ColliderBuilder.New().BoxShape(0.5f, 0.5f).Create());
            cursor.Add(new StayOnMouse());

            var test = new Entity();
            var transform = new Transform()
            {
                Position = new Vector2(1)
            };
            test.Add(transform);
            var sprite = new Sprite()
            {
                Texture = TextureUtil.CreateTexture(64, 64, Color.Red)
            };
            test.Add(sprite);
            
            var collider = ColliderBuilder.New().IsDynamic()
                .Create();
            test.Add(collider);

            var camera = new Entity();
            camera.Add(new Transform());
            camera.Add(new Camera());

            var animation = AnimationBuilder.New()
                .InsertFrame(0, new KeyFrame(new Vector2(-5, 0)))
                .InsertFrame(2, new KeyFrame(new Vector2(5, 0)))
                .InsertFrame(3, new KeyFrame(new Vector2(5, 2.5f)))
                .Reverses(true)
                .Loops(true)
                .AnimatePhysics()
                .Create();

            var animationContainer= new AnimationContainer();
            animationContainer.Animation.Add("test", animation);

            var animated = new Entity();
            animated.Add(new Transform());
            animated.Add(animationContainer);
            animated.Add(ColliderBuilder.New().IsDynamic().Create());
            animated.Add(new Sprite()
            {
                Texture = TextureUtil.CreateTexture(64, 64, Color.Pink)
            });

            scene.Engine.AddEntity(cursor);
            scene.Engine.AddEntity(test);
            scene.Engine.AddEntity(camera);
            scene.Engine.AddEntity(animated);
            scene.Engine.Systems.Get<CameraSystem>().ActiveCamera = camera;

            scene.Engine.MessageHub.SendMessage(new StartAnimationMessage(animated, "test"));
        }

        protected override void Update(GameTime gameTime)
        {
            time.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            scene.Update(time);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            scene.Draw();
            base.Draw(gameTime);
        }
    }
}
