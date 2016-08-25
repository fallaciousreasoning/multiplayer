using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MultiPlayer;
using XamlEditor.Extensions;

namespace XamlEditor.ViewModels
{
    public class HierarchyViewModel : BaseViewModel
    {
        private GameObject root;
        private bool isExpanded;
        private bool isSelected;

        public GameObject Root
        {
            get { return root; }
            set
            {
                if (Equals(value, root)) return;
                root = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));

                LoadChildren();
            }
        }

        public string Name
        {
            get { return Root.Name; }
            set
            {
                if (Equals(value, root.Name)) return;
                Root.Name = value;
                OnPropertyChanged();
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

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                OnPropertyChanged();

                OnSelected?.Invoke(Root);
            }
        }

        public Action<GameObject> OnSelected;

        public ObservableCollection<HierarchyViewModel> Children { get; } = new ObservableCollection<HierarchyViewModel>();

        public HierarchyViewModel()
        {
        }

        public void Reload()
        {
            LoadChildren();
        }

        private void LoadChildren()
        {
            if (Root == null) return;

            Children.Clear();

            var children = Root.GetComponents<GameObject>();
            children.Foreach(child =>
            {
                Children.Add(new HierarchyViewModel()
                {
                    OnSelected = OnSelected,
                    Root = child
                });
            });
        }
    }
}
