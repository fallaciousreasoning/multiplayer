using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using XamlEditor.Extensions;

namespace XamlEditor.ViewModels
{
    public class EntityViewModel : BaseViewModel
    {
        private Entity entity;

        public Entity Entity
        {
            get { return entity; }
            set
            {
                if (Equals(value, entity)) return;
                entity = value;
                OnPropertyChanged();

                Reload();
            }
        }

        public EntityViewModel()
        {
            var g = new Entity();
            g.Add(new Transform());

            Entity = g;
        }

        public void Reload()
        {
            Components.Clear();
            //Add all the scripts to the view model
            entity?.Components
                .Where(
                    c => 
                        !c.GetType().GetAllAttributes().Any(a => a is EditorIgnoreAttribute)
                    )
                .Foreach(c =>
                    Components.Add(new ComponentViewModel(c))
                );
        }

        public ObservableCollection<ComponentViewModel> Components { get; private set; } = new ObservableCollection<ComponentViewModel>();
    }
}
