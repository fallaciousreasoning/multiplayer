using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MultiPlayer
{
    public class EnigmaGame : Game
    {
        private Scene scene;
        private GraphicsDeviceManager graphics;

        public EnigmaGame(Scene scene)
        {
            graphics = new GraphicsDeviceManager(this);

            this.scene = scene;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            scene.Device = GraphicsDevice;

            scene.Start();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            scene.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            scene.Draw();
            base.Draw(gameTime);
        }
    }
}
