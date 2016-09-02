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
    public class PrimitiveViewModel : BaseViewModel, IValueViewModel
    {
        private static readonly List<ISupportableConverter> converters = new List<ISupportableConverter>()
        {
            new StringNumberConverter(),
            new StringBooleanConverter(),
            new StringStringConverter(),
        };

        private IAccessor accessor;

        private object o;

        private IValueConverter converter;

        private string FieldName => accessor?.Name;

        public void Reload()
        {
            Value = accessor.GetValue(Object);
        }

        public string Name => FieldName;
        public Type ValueType => Accessor?.ValueType;

        public IAccessor Accessor
        {
            get { return accessor; }
            set
            {
                converter = GetConverter(value.ValueType);
                if (converter == null) throw new ArgumentException($"Unsupported type {value.ValueType.Name}");

                if (Equals(value, accessor)) return;
                accessor = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
            }
        }

        public object Value
        {
            get { return accessor?.GetValue(Object); }
            set
            {
                try
                {
                    var convertedValue = GetConvertedValue(value);
                    accessor?.SetValue(Object, convertedValue);
                }
                catch (ArgumentException)
                {
                    throw new Exception($"{value.GetType().Name} must be a {ValueType.Name}");
                }
            }
        }

        public ObservableCollection<IValueViewModel> Children { get; } = null;

        public object Object
        {
            get { return o; }
            set
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
    }
}
