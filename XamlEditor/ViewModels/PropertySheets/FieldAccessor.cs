using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels.PropertySheets
{
    public class FieldAccessor : IAccessor
    {
        private readonly FieldInfo fieldInfo;

        public FieldAccessor(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public Type ValueType => fieldInfo.FieldType;
        public string Name => fieldInfo.Name;

        public void SetValue(object on, object value)
        {
            fieldInfo.SetValue(on, value);
        }

        public object GetValue(object from)
        {
            return fieldInfo.GetValue(from);
        }
    }
}
