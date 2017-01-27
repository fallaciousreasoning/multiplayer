using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MultiPlayer.Assets
{
    public class AssetManager
    {
        private readonly Dictionary<string, (int count, object asset)> assetTracker = new Dictionary<string, ValueTuple<int, object>>();

        public AssetManager()
        {
            
        }
    }
}
