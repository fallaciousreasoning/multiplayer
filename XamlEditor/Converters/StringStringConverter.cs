using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace XamlEditor.Converters
{
    public class StringStringConverter : ISupportableConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
                return value.ToString();
            if (targetType == typeof(char))
            {
                var s = value.ToString();
                if (s.Length != 1) throw new ArgumentException($"String should be exactly one character long for character conversion");
                return s[0];
            }

            throw new ArgumentException($"Unsupported conversion to {targetType.Name}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new ArgumentException($"Unsupported conversion to {targetType.Name}");
            return value.ToString();
        }

        public bool Supports(Type targetType)
        {
            return new[] {typeof(string), typeof(char)}.Contains(targetType);
        }
    }
}
