using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels.PropertySheets
{
    public class PropertyAccessor : IAccessor
    {
        private readonly PropertyInfo propertyInfo;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public Type ValueType => propertyInfo.PropertyType;
        public string Name => propertyInfo.Name;

        public void SetValue(object on, object value)
        {
            propertyInfo.SetValue(on, value);
        }

        public object GetValue(object from)
        {
            return propertyInfo.GetValue(from);
        }
    }
}
