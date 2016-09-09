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
            get { return entity.FriendlyName; }
            set
            {
                if (value == entity.FriendlyName) return;
                entity.FriendlyName = value;
                OnPropertyChanged();
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
                OnPropertyChanged(nameof(Name));
            }
        }

        public Action<Entity> OnSelected;
        private Entity entity;
    }
}
