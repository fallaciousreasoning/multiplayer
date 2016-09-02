using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Animation.Components
{
    public class KeyFrame
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        internal KeyFrame(Dictionary<string, object> values)
        {
            this.values = values;
        }

        public object GetValue(string series)
        {
            return values[series];
        }
    }
}
