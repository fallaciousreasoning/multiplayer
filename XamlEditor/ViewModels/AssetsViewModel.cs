using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.GameComponents.Physics;

namespace XamlEditor.ViewModels
{
    public class AssetsViewModel : BaseViewModel
    {
        public ObservableCollection<Type> Scripts { get; } = new ObservableCollection<Type>();

        public void LoadScripts()
        {
            Scripts.Clear();

            Scripts.Add(typeof(Sprite));
            Scripts.Add(typeof(Collider));
        }
    }
}
