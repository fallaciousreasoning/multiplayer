using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer.Core;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.InputMethods;

namespace MultiPlayer
{
    public class Scene
    {
        public Scene(IMouse mouse, IKeyboard keyboard)
        {
            Input = new InputManager(mouse, keyboard);

            Engine = new Engine();
        }

        public virtual void Start()
        {
            SpriteBatch = new SpriteBatch(Device);
            ActiveScene = this;

            Engine.Start();
        }

        public void Update(Time time)
        {
            this.Time = time;
            Engine.Update(time);
        }

        public void Draw()
        {
            Engine.Draw();
        }

        public Time Time { get; private set; }
        public Engine Engine { get; private set; }
        public InputManager Input { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public GraphicsDevice Device { get; set; }
        public static Scene ActiveScene { get; set; }
    }
}
