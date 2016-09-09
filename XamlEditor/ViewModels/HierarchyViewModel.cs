using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MultiPlayer;
using MultiPlayer.Core;
using XamlEditor.Extensions;

namespace XamlEditor.ViewModels
{
    public class HierarchyViewModel : BaseViewModel
    {
        private Engine engine;

        public Engine Engine
        {
            get { return engine; }
            set
            {
                if (Equals(value, engine)) return;
                engine = value;
                OnPropertyChanged();

                LoadChildren();
            }
        }

        public EntityHierarchyViewModel ActiveItem
        {
            get { return activeItem; }
            set
            {
                if (Equals(value, activeItem)) return;
                activeItem = value;
                OnPropertyChanged();

                ActiveItem?.OnSelected(ActiveItem.Entity);
            }
        }

        public Action<Entity> OnSelected;
        private EntityHierarchyViewModel activeItem;

        public ObservableCollection<EntityHierarchyViewModel> Children { get; } = new ObservableCollection<EntityHierarchyViewModel>();

        public HierarchyViewModel()
        {
        }

        public void Reload()
        {
            LoadChildren();
        }

        private void LoadChildren()
        {
            if (Engine == null) return;

            Children.Clear();

            Engine.Entities
                .Foreach(child =>
            {
                Children.Add(new EntityHierarchyViewModel()
                {
                    OnSelected = OnSelected,
                    Entity = child
                });
            });
        }
    }
}
