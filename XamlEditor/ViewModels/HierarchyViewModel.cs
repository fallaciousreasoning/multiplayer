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

        public GameObject Root
        {
            get { return root; }
            set
            {
                if (Equals(value, root)) return;
                root = value;
                OnPropertyChanged();

                LoadChildren();
            }
        }

        public ObservableCollection<HierarchyViewModel> Children { get; } = new ObservableCollection<HierarchyViewModel>();

        public RoutedCommand SelectedCommand;
        public RoutedCommand DeselectedCommand;

        public HierarchyViewModel()
        {
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
                    Root = child,
                });
            });
        }
    }
}
