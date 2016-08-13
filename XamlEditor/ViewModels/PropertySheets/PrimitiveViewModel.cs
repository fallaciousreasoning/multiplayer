using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XamlEditor.Annotations;
using XamlEditor.Converters;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.ViewModels
{
    public class PrimitiveViewModel : BaseViewModel, IPropertyViewModel
    {
        private static readonly List<ISupportableConverter> converters = new List<ISupportableConverter>()
        {
            new StringNumberConverter(),
            new StringBooleanConverter(),
            new StringStringConverter(),
        };

        private PropertyInfo propertyInfo;
        private FieldInfo fieldInfo;

        private object o;

        private IValueConverter converter;

        private string PropertyName => propertyInfo?.Name;
        private string FieldName => fieldInfo?.Name;

        public string Name => PropertyName ?? FieldName;
        public Type ValueType => FieldInfo?.FieldType ?? PropertyInfo?.PropertyType;

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
            private set
            {
                converter = GetConverter(value.PropertyType);
                if (converter == null) throw new ArgumentException($"Unsupported type {value.PropertyType.Name}");

                if (Equals(value, propertyInfo)) return;
                propertyInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
            }
        }

        public FieldInfo FieldInfo
        {
            get { return fieldInfo; }
            private set
            {
                converter = GetConverter(value.FieldType);
                if (converter == null) throw new ArgumentException($"Unsupported type {value.FieldType.Name}");

                if (Equals(value, fieldInfo)) return;
                fieldInfo = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
            }
        }

        public object Value
        {
            get { return propertyInfo != null ? propertyInfo.GetValue(Object) : fieldInfo.GetValue(Object); }
            set
            {
                try
                {
                    var convertedValue = GetConvertedValue(value);
                    propertyInfo?.SetValue(Object, convertedValue);
                    fieldInfo?.SetValue(Object, convertedValue);
                }
                catch (ArgumentException)
                {
                    throw new Exception($"{value.GetType().Name} must be a {ValueType.Name}");
                }
            }
        }

        public ObservableCollection<IPropertyViewModel> Children { get; } = null;

        public object Object
        {
            get { return o; }
            private set
            {
                if (Equals(value, o)) return;
                o = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        private object GetConvertedValue(object value)
        {
            if (value.GetType() == ValueType) return value;
            return GetConverter(ValueType).Convert(value, ValueType, null, CultureInfo.CurrentCulture);
        }

        private static ISupportableConverter GetConverter(Type type)
        {
            var converter = converters.FirstOrDefault(c => c.Supports(type));
            return converter;
        }

        public static bool CanConvert(Type type)
        {
            return GetConverter(type) != null;
        }

        private PrimitiveViewModel() { }

        public static PrimitiveViewModel Create(object o, PropertyInfo propertyInfo)
        {
            return new PrimitiveViewModel()
            {
                Object = o,
                propertyInfo = propertyInfo
            };
        }

        public static PrimitiveViewModel Create(object o, FieldInfo fieldInfo)
        {
            return new PrimitiveViewModel()
            {
                Object = o,
                fieldInfo = fieldInfo
            };
        }
    }
}
