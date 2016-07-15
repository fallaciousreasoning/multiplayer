using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.GameComponents;

namespace Editor.Actions
{
    public class PlaceBlockAction : IEditorAction, IKnowsMapInfo, IKnowsScene, IKnowsPrefabManager
    {
        private GameObject gameObject;
        
        private readonly PrefabInfo prefabInfo;

        public PlaceBlockAction(PrefabInfo prefabInfo)
        {
            this.prefabInfo = prefabInfo;
        }

        public void Do()
        {
            gameObject = Prefabs.Instantiate(prefabInfo, Scene);

            Info.Scene.Children.Add(prefabInfo);
        }

        public void Undo()
        {
            gameObject.ShouldRemove = true;
            Info.Scene.Children.Remove(prefabInfo);
        }

        public MapInfo Info { get; set; }
        public GameObject Scene { get; set; }
        public PrefabFactory Prefabs { get; set; }
    }
}
