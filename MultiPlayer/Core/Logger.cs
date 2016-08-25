using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IDrawable = MultiPlayer.GameComponents.IDrawable;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;

namespace MultiPlayer.Core
{
    public class Logger : IUpdateable, IDrawable
    {
        private static Logger instance;

        private readonly List<Message> messages = new List<Message>();
        private float keepFor = 60;

        public Logger()
        {
            instance = this;
        }

        public void WriteMessage(object msg)
        {
            Debug.WriteLine(msg);
            Console.WriteLine(msg);

            messages.Add(new Message(msg) {Life = keepFor});
        }

        private class Message
        {
            public object Content { get; set; }
            public float Life;
            public Color Color { get; set; }

            public Message(object content)
            {
                Color = Color.White;
                this.Content = content;
            }

            public override string ToString()
            {
                return Content.ToString();
            }
        }

        public void Update(float step)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                var msg = messages[i];
                msg.Life -= step;

                if (msg.Life < 0)
                {
                    messages.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw()
        {
            var spacing = 20;
            for (var i = 0; i < messages.Count; ++i)
            {
                var y = Scene.ActiveScene.Device.Viewport.Height - spacing*(messages.Count - i);
                var x = 10;

                Scene.ActiveScene.SpriteBatch.DrawString(Scene.ActiveScene.DefaultFont, messages[i].ToString(), new Vector2(x, y), messages[i].Color);
            }
        }

        public static void WriteLine(object msg)
        {
            instance.WriteMessage(msg);
        }

        public static void Log(object msg)
        {
            WriteLine(msg);
        }
    }
}
