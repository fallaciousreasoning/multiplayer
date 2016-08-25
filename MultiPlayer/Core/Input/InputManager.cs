using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core.InputMethods;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using IUpdateable = MultiPlayer.GameComponents.IUpdateable;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

namespace MultiPlayer.Core
{
    public enum MouseButton
    {
        Left, Right, Middle
    }

    public class InputManager : IUpdateable
    {
        private IKeyboard keyboardState;
        private IKeyboard oldKeyState;

        private IMouse mouseState;
        private IMouse oldMouseState;

        private readonly Dictionary<string, InputAxis> inputAxes = new Dictionary<string, InputAxis>();
        private readonly Dictionary<string, InputButton> inputButtons = new Dictionary<string, InputButton>(); 

        public InputManager(IMouse mouse, IKeyboard keyboard)
        {
            this.mouseState = mouse;
            this.keyboardState = keyboard;

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

        public void Update(float step)
        {
            oldKeyState = keyboardState.Clone();
            oldMouseState = mouseState.Clone();

            keyboardState.Update();
            mouseState.Update();
        }

        public Vector2 MousePosition { get { return new Vector2(mouseState.MousePosition.X, mouseState.MousePosition.Y); } }
        public Vector2 LastMousePosition { get { return new Vector2(oldMouseState.MousePosition.X, oldMouseState.MousePosition.Y); } }
        public Vector2 MouseMoved { get { return MousePosition - LastMousePosition; } }

        public bool IsDown(MouseButton button)
        {
            return mouseState.IsKeyDown(button);
        }

        public bool IsDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool IsUp(MouseButton button)
        {
            return mouseState.IsKeyUp(button);
        }

        public bool IsUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public bool IsPressed(MouseButton button)
        {
            return mouseState.IsKeyDown(button) && oldMouseState.IsKeyUp(button);
        }

        public bool IsPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        public bool IsReleased(MouseButton button)
        {
            return oldMouseState.IsKeyDown(button) && mouseState.IsKeyUp(button);
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
