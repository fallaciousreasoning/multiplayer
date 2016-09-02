using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.Core;
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
                    scene.UpdateNotifier.ComponentAdded -= OnComponentAdded;
                    scene.UpdateNotifier.ComponentRemoved -= OnComponentRemoved;
                    scene.PropertyChanged -= HeardEditPropertyChanged;
                }

                scene = value;

                if (scene != null)
                {
                    scene.UpdateNotifier.ChildAdded += OnChildAdded;
                    scene.UpdateNotifier.ChildRemoved += OnChildRemoved;
                    scene.UpdateNotifier.ComponentAdded += OnComponentAdded;
                    scene.UpdateNotifier.ComponentRemoved += OnComponentRemoved;
                    scene.UpdateNotifier.ComponentChanged += EntityViewModel.ComponentChanged;
                    scene.PropertyChanged += HeardEditPropertyChanged;
                }

                EntityViewModel.Entity = null;
                HierarchyViewModel.Engine = scene?.Engine;
            }
        }

        public EntityViewModel EntityViewModel { get; } = new EntityViewModel();
        public HierarchyViewModel HierarchyViewModel { get; } = new HierarchyViewModel();

        public SceneViewModel()
        {
            HierarchyViewModel.OnSelected = OnGameObjectSelected;
        }

        public void OnGameObjectSelected(Entity entity)
        {
            EntityViewModel.Entity = entity;
            Scene.SelectGameObject(entity);
        }

        public void HeardEditPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Scene.Engine))
                HierarchyViewModel.Engine = Scene.Engine;
        }

        public void OnChildAdded(Entity parent, Entity child)
        {
            HierarchyViewModel.Reload();
        }

        public void OnChildRemoved(Entity parent, Entity child)
        {
            HierarchyViewModel.Reload();
        }

        public void OnComponentAdded(Entity parent, object script)
        {
            if (parent == EntityViewModel.Entity)
                EntityViewModel.Reload();
        }

        public void OnComponentRemoved(Entity parent, object script)
        {
            if (parent == EntityViewModel.Entity)
                EntityViewModel.Reload();
        }

    }
}
