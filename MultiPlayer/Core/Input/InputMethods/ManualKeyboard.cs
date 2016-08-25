using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MultiPlayer.Core.InputMethods
{
    public class ManualKeyboard : IKeyboard
    {
        private readonly Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();

        public bool IsKeyDown(Keys key)
        {
            if (!keys.ContainsKey(key)) return false;
            return keys[key];
        }

        public bool IsKeyUp(Keys key)
        {
            return !IsKeyDown(key);
        }

        public void SetKeyState(Keys key, bool state)
        {
            if (!keys.ContainsKey(key)) keys.Add(key, state);
            keys[key] = state;
        }

        public void Update()
        {

        }

        public IKeyboard Clone()
        {
            var state = new ManualKeyboard();
            foreach (var key in keys.Keys)
                state.keys.Add(key, keys[key]);

            return state;
        }
    }
}
