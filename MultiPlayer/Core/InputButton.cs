using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MultiPlayer.Core
{
    public class InputButton
    {
        public Keys[] ActiveKeys;
        public MouseButton[] ActiveButtons;

        public InputButton()
            :this(new Keys[0],new MouseButton[0])
        {
        }

        public InputButton(params Keys[] activeKeys)
            :this(activeKeys, new MouseButton[0])
        { }

        public InputButton(Keys[] activeKeys, MouseButton[] activeButtons)
        {
            this.ActiveButtons = activeButtons;
            this.ActiveKeys = activeKeys;
        }

        public bool GetButton(InputManager manager)
        {
            return ActiveKeys.Any(manager.IsDown) || ActiveButtons.Any(manager.IsDown);
        }
    }
}
