using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels
{
    public class ComponentViewModel : ComplexViewModel
    {
        public ComponentViewModel(object o)
            : base()
        {
            this.Value = o;
        }
    }
}
