using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.Systems;

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

            scene.Engine.AddSystem(new SpriteRenderer());

            var test = new Entity();
            var transform = new TransformComponent()
            {
                Position = new Vector2(1)
            };
            test.Add(transform);
            var sprite = new SpriteComponent()
            {
                Texture = TextureUtil.CreateTexture(64, 64, Color.Red)
            };
            test.Add(sprite);
            scene.Engine.AddEntity(test);
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
