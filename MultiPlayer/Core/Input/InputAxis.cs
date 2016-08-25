using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using MultiPlayer.Core.Input;

namespace MultiPlayer.Core
{
    public class InputAxis
    {
        public InputButton PositiveButton;
        public InputButton NegativeButton;

        public Keys[] PositiveKeys
        {
            get { return PositiveButton.ActiveKeys; }
            set { PositiveButton.ActiveKeys = value; }
        }

        public Keys[] NegativeKeys
        {
            get { return NegativeButton.ActiveKeys; }
            set { NegativeButton.ActiveKeys = value; }
        }

        public MouseButton[] PositiveMouseButtons
        {
            get { return PositiveButton.ActiveButtons; }
            set { PositiveButton.ActiveButtons = value; }
        }

        public MouseButton[] NegativeMouseButtons
        {
            get { return NegativeButton.ActiveButtons; }
            set { NegativeButton.ActiveButtons = value; }
        }

        public InputAxis()
            :this(new Keys[0], new MouseButton[0], new Keys[0], new MouseButton[0])
        {
            
        }

        public InputAxis(Keys[] positiveKeys, Keys[] negativeKeys)
            :this(positiveKeys, new MouseButton[0], negativeKeys, new MouseButton[0])
        {
            
        }

        public InputAxis(Keys[] positiveKeys, MouseButton[] positiveMouseButtons, Keys[] negativeKeys, MouseButton[] negativeMouseButtons)
            :this(new InputButton(positiveKeys, positiveMouseButtons), new InputButton(negativeKeys, negativeMouseButtons))
        {
        }

        public InputAxis(InputButton positiveButton, InputButton negativeButton)
        {
            this.PositiveButton = positiveButton;
            this.NegativeButton = negativeButton;
        }

        public int GetValue(InputManager manager)
        {
            var value = 0;
            if (PositiveButton.GetButton(manager)) value++;
            if (NegativeButton.GetButton(manager)) value--;

            return value;

        }
    }
}
