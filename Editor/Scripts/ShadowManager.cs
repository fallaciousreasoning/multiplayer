using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;

namespace Editor.Scripts
{
    public class ShadowManager
    {
        private readonly Dictionary<string, GameObject> shadows = new Dictionary<string, GameObject>();

        public void AddShadow(string prefabName, GameObject shadow)
        {
            shadows.Add(prefabName, shadow);
            //shadow.Visible = false;
        }

        public GameObject GetShadow(string prefabName)
        {
            return shadows[prefabName];
        }

        public GameObject this[string prefabName]
        {
            get { return GetShadow(prefabName); }
            set
            {
                if (!shadows.ContainsKey(prefabName)) AddShadow(prefabName, value);
                else shadows[prefabName] = value;
                //value.Visible = false;
            }
        }
    }
}
