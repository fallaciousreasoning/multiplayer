using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace XamlEditor.Converters
{
    public class StringBooleanConverter : ISupportableConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool)) throw new ArgumentException($"Unsupported conversion to {targetType.Name}");
            return string.Equals("true", value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new ArgumentException($"Unsupported conversion to {targetType.Name}");
            return value.ToString();
        }

        public bool Supports(Type targetType)
        {
            return targetType == typeof(bool);
        }
    }
}
