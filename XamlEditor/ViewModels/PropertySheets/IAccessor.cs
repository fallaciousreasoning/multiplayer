using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels.PropertySheets
{
    public interface IAccessor
    {
        Type ValueType { get; }
        string Name { get; }

        void SetValue(object on, object value);
        object GetValue(object from);
    }
}
