using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using XamlEditor.Annotations;
using XamlEditor.Scenes;

namespace XamlEditor.ViewModels
{
    public class SceneViewModel : BaseViewModel
    {
        private EditScene scene;

        public EditScene Scene
        {
            get { return scene; }
            set
            {
                if (scene != null)
                {
                    scene.UpdateNotifier.ChildAdded -= OnChildAdded;
                    scene.UpdateNotifier.ChildRemoved -= OnChildRemoved;
                    scene.UpdateNotifier.ScriptAdded -= OnScriptAdded;
                    scene.UpdateNotifier.ScriptRemoved -= OnScriptRemoved;
                    scene.PropertyChanged -= HeardEditPropertyChanged;
                }

                scene = value;

                if (scene != null)
                {
                    scene.UpdateNotifier.ChildAdded += OnChildAdded;
                    scene.UpdateNotifier.ChildRemoved += OnChildRemoved;
                    scene.UpdateNotifier.ScriptAdded += OnScriptAdded;
                    scene.UpdateNotifier.ScriptRemoved += OnScriptRemoved;
                    scene.PropertyChanged += HeardEditPropertyChanged;
                }

                GameObjectViewModel.GameObject = null;
                HierarchyViewModel.Root = scene?.SceneRoot;
            }
        }

        public GameObjectViewModel GameObjectViewModel { get; } = new GameObjectViewModel();
        public HierarchyViewModel HierarchyViewModel { get; } = new HierarchyViewModel();

        public SceneViewModel()
        {
            HierarchyViewModel.OnSelected = OnGameObjectSelected;
        }

        public void OnGameObjectSelected(GameObject gameObject)
        {
            GameObjectViewModel.GameObject = gameObject;
            Scene.SelectGameObject(gameObject);
        }

        public void HeardEditPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Scene.SceneRoot))
                HierarchyViewModel.Root = Scene.SceneRoot;
        }

        public void OnChildAdded(GameObject parent, GameObject child)
        {
            HierarchyViewModel.Reload();
        }

        public void OnChildRemoved(GameObject parent, GameObject child)
        {
            HierarchyViewModel.Reload();
        }

        public void OnScriptAdded(GameObject parent, object script)
        {
            if (parent == GameObjectViewModel.GameObject)
                GameObjectViewModel.Reload();
        }

        public void OnScriptRemoved(GameObject parent, object script)
        {
            if (parent == GameObjectViewModel.GameObject)
                GameObjectViewModel.Reload();
        }

    }
}
