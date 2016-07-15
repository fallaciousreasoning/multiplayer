using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;

namespace Editor.Scripts
{
    public class BlockInfo
    {
        private readonly Dictionary<string, GameObject> shadows = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, bool> sizeables = new Dictionary<string, bool>();

        public void AddInfo(string prefabName, GameObject shadow, bool sizable)
        {
            shadows.Add(prefabName, shadow);
            sizeables.Add(prefabName, sizable);
        }

        public GameObject GetShadow(string prefabName)
        {
            return shadows[prefabName];
        }

        public bool IsSizable(string prefabName)
        {
            return sizeables[prefabName];
        }
    }
}
