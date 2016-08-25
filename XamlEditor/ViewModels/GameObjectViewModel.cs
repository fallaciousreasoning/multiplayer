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
using XamlEditor.Extensions;

namespace XamlEditor.ViewModels
{
    public class GameObjectViewModel : BaseViewModel
    {
        private GameObject gameObject;

        public GameObject GameObject
        {
            get { return gameObject; }
            set
            {
                if (Equals(value, gameObject)) return;
                gameObject = value;
                OnPropertyChanged();

                Reload();
            }
        }

        public GameObjectViewModel()
        {
            var g = GameObjectFactory
                .New()
                .AtPosition(new Vector2(20, 10))
                .AtScale(2);
            GameObject = g.Create();
        }

        public void Reload()
        {
            Scripts.Clear();
            //Add all the scripts to the view model
            gameObject?.Components
                .Where(
                    c => 
                        c.Key != typeof(GameObject) &&
                        !c.Key.GetAllAttributes().Any(a => a is EditorIgnoreAttribute)
                    )
                .Foreach(list =>
                    list.Value.Foreach(script => Scripts.Add(new ScriptViewModel(script)))
                );
        }

        public ObservableCollection<ScriptViewModel> Scripts { get; private set; } = new ObservableCollection<ScriptViewModel>();
    }
}
