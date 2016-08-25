using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.Annotations;
using MultiPlayer.GameComponents;

namespace XamlEditor.GameComponents
{
    public delegate void ScriptEvent(GameObject gameObject, object script);
    public delegate void ChildEvent(GameObject gameObject, GameObject child);

    [EditorIgnore]
    public class UpdateNotifier : IHearsAdd, IHearsChildDestroyed, IKnowsGameObject
    {
        public event ScriptEvent ScriptAdded;
        public event ChildEvent ChildAdded;

        public event ScriptEvent ScriptRemoved;
        public event ChildEvent ChildRemoved;

        public void OnAdded(object o)
        {
            if (o is GameObject)
                ChildAdded?.Invoke(GameObject, (GameObject) o);
            else ScriptAdded?.Invoke(GameObject, o);
        }
        
        public void OnChildDestroyed(object child)
        {
            if (child is GameObject)
                ChildRemoved?.Invoke(GameObject, (GameObject)child);
            else ScriptRemoved?.Invoke(GameObject, child);
        }

        public GameObject GameObject { get; set; }
    }
}
