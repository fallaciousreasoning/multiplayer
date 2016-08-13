using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels.PropertySheets
{
    public interface IPropertyViewModel
    {
        string Name { get; }
        object Value { get; set; }
        object Object { get; set; }
        PropertyInfo PropertyInfo { get; set; }

        ObservableCollection<IPropertyViewModel> Children { get; }
    }
}
