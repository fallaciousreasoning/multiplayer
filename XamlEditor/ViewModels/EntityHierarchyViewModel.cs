using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;

namespace XamlEditor.ViewModels
{
    public class EntityHierarchyViewModel : BaseViewModel
    {
        public string Name
        {
            get { return friendlyName; }
            set
            {
                if (value == friendlyName) return;
                friendlyName = value;
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

                OnSelected?.Invoke(Entity);
            }
        }

        public Entity Entity
        {
            get { return entity; }
            set
            {
                if (Equals(value, entity)) return;
                entity = value;
                OnPropertyChanged();
            }
        }

        public Action<Entity> OnSelected;
        private Entity entity;
        private string friendlyName;
        private bool isSelected;
    }
}
