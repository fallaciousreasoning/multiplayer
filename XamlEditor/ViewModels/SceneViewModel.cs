using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XamlEditor.Annotations;
using XamlEditor.Scenes;

namespace XamlEditor.ViewModels
{
    public class SceneViewModel : BaseViewModel
    {
        public EditScene Scene { get; set; }

        public GameObjectViewModel GameObjectViewModel { get; set; } = new GameObjectViewModel();
    }
}
