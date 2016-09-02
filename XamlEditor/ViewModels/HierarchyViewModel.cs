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
        private bool isExpanded;

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

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value == isExpanded) return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public Action<Entity> OnSelected;

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
