using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core.InputMethods;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;

namespace MultiPlayer.Core.Input
{
    public enum MouseButton
    {
        Left, Right, Middle
    }

    public class InputManager : ISystem, IRegistrableSystem
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

        protected void Update()
        {
            oldKeyState = keyboardState.Clone();
            oldMouseState = mouseState.Clone();

            keyboardState.Update();
            mouseState.Update();
        }

        public Vector2 MouseScreenPosition => new Vector2(mouseState.MousePosition.X, mouseState.MousePosition.Y);
        public Vector2 LastMouseScreenPosition => new Vector2(oldMouseState.MousePosition.X, oldMouseState.MousePosition.Y);
        public Vector2 MouseScreenMoved => MouseScreenPosition - LastMouseScreenPosition;

        public Vector2 MousePosition => CameraSystem.Active.ScreenToWorld(MouseScreenPosition);
        public Vector2 LastMousePosition => CameraSystem.Active.ScreenToWorld(LastMouseScreenPosition);
        public Vector2 MouseMoved => MousePosition - LastMousePosition;

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

        public void RecieveMessage(IMessage message)
        {
            if (message is UpdateMessage) Update();
        }

        public IEnumerable<Type> Receives { get; } = new[] {typeof(UpdateMessage)};
    }
}
