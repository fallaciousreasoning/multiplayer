using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XamlEditor.Converters
{
    public interface ISupportableConverter : IValueConverter
    {
        bool Supports(Type targetType);
    }
}
