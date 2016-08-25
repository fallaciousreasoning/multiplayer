using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XamlEditor.Converters
{
    public class StringNumberConverter : ISupportableConverter
    {
        private static readonly Type[] supportedTypes = {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(float),
            typeof(double)
        };

        public bool Supports(Type targetType)
        {
            return supportedTypes.Contains(targetType);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(string)) throw new ArgumentException($"Unsupported input type {value.GetType().Name}");
            if (!supportedTypes.Contains(targetType)) throw new ArgumentException($"Unsupported target type {targetType.Name}");

            var result = double.Parse(value.ToString());
            return System.Convert.ChangeType(result, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new ArgumentException();
            
            return value.ToString();
        }
    }
}
