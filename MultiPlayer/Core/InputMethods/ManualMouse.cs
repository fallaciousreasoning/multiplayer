using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MultiPlayer.Core.InputMethods
{
    public class ManualMouse : IMouse
    {
        public Vector2 MousePosition { get; set; }
        private readonly Dictionary<MouseButton, ButtonState> buttonStates = new Dictionary<MouseButton, ButtonState>();

        public bool IsKeyDown(MouseButton button)
        {
            return GetButtonState(button) == ButtonState.Pressed;
        }

        public bool IsKeyUp(MouseButton button)
        {
            return GetButtonState(button) == ButtonState.Released;
        }

        public void Update()
        {
        }

        public ButtonState GetButtonState(MouseButton button)
        {
            return buttonStates.ContainsKey(button) ? buttonStates[button] : ButtonState.Released;
        }

        public void SetButtonState(MouseButton button, ButtonState state)
        {
            if (!buttonStates.ContainsKey(button)) buttonStates.Add(button, ButtonState.Released);
            buttonStates[button] = state;
        }

        public IMouse Clone()
        {
            var state = new ManualMouse();
            foreach (var key in buttonStates.Keys)
            {
                state.buttonStates.Add(key, buttonStates[key]);
            }
            return state;
        }
    }
}
