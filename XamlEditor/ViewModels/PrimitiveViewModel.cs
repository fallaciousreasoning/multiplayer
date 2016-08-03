using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XamlEditor.Converters;

namespace XamlEditor.ViewModels
{
    public class PrimitiveViewModel : BaseViewModel
    {
        private static readonly List<ISupportableConverter> converters = new List<ISupportableConverter>()
        {
            new StringNumberConverter(),
            new StringBooleanConverter()
        };

        private PropertyInfo propertyInfo;
        private object o;

        private IValueConverter converter;

        public string PropertyName => propertyInfo.Name;

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
            set
            {
                converter = converters.FirstOrDefault(c => c.Supports(value.PropertyType));
                if (converter == null && value.PropertyType != typeof(string)) throw new ArgumentException($"Unsupported type {value.PropertyType.Name}");

                if (Equals(value, propertyInfo)) return;
                propertyInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertyName));
                OnPropertyChanged(nameof(Value));
            }
        }

        public object Value
        {
            get { return propertyInfo.GetValue(Object); }
            set
            {
                try
                {
                    var convertedValue = GetConvertedValue(value);
                    propertyInfo.SetValue(Object, convertedValue);
                }
                catch (ArgumentException)
                {
                    throw new Exception($"{propertyInfo.Name} must be a {propertyInfo.PropertyType.Name}");
                }
            }
        }

        public object Object
        {
            get { return o; }
            set
            {
                if (Equals(value, o)) return;
                o = value;
                OnPropertyChanged();
            }
        }

        private object GetConvertedValue(object value)
        {
            if (value.GetType() == propertyInfo.PropertyType) return value;
            return converter.Convert(value, PropertyInfo.PropertyType, null, CultureInfo.CurrentCulture);
        }
    }
}
