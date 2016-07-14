using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core;
using OpenTK.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

namespace MultiPlayer
{
    public enum MouseButton
    {
        Left, Right, Middle
    }

    public class InputManager : GameComponent
    {
        private KeyboardState keyboardState;
        private KeyboardState oldKeyState;

        private MouseState mouseState;
        private MouseState oldMouseState;

        private readonly Dictionary<string, InputAxis> inputAxes = new Dictionary<string, InputAxis>();
        private readonly Dictionary<string, InputButton> inputButtons = new Dictionary<string, InputButton>(); 

        public InputManager(Game game) : base(game)
        {
            AddInputAxis("horizontal", new InputAxis()
            {
                PositiveKeys = new[]
                {
                    Keys.Right,
                    Keys.D,
                },
                NegativeKeys = new[]
                {
                    Keys.A,
                    Keys.Left
                }
            });

            AddInputAxis("vertical", new InputAxis()
            {
                PositiveKeys = new []
                {
                    Keys.Down,
                    Keys.S,
                },
                NegativeKeys = new[]
                {
                    Keys.Up, 
                    Keys.W,
                }
            });
        }

        public void AddButton(string name, InputButton button)
        {
            inputButtons.Add(name, button);
        }

        public void AddInputAxis(string name, InputAxis axis)
        {
            inputAxes.Add(name, axis);
        }

        public override void Update(GameTime gameTime)
        {
            oldKeyState = keyboardState;
            oldMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        public Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public Vector2 LastMousePosition { get { return new Vector2(oldMouseState.X, oldMouseState.Y); } }
        public Vector2 MouseMoved { get { return MousePosition - LastMousePosition; } }

        public bool IsDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                default:
                    return mouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public bool IsDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool IsUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Released;
                default:
                    return mouseState.LeftButton == ButtonState.Released;
            }
        }

        public bool IsUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public bool IsPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
                default:
                    return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
            }
        }

        public bool IsPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        public bool IsReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
                default:
                    return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public bool IsReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyState.IsKeyDown(key);
        }

        public bool GetButtonDown(string name)
        {
            return inputButtons[name].GetButton(this);
        }

        public int GetAxis(string name)
        {
            return inputAxes[name].GetValue(this);
        }
    }
}
