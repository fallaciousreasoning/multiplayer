using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XamlEditor.Properties;
using XamlEditor.ViewModels;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.TemplateSelectors
{
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        private static readonly Type[] Numerics = {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(float),
            typeof(double)
        };

        public DataTemplate UnknownDataTemplate { get; set; }

        public DataTemplate ArrayDataTemplate { get; set; }

        public DataTemplate ComplexDataTemplate { get; set; }

        public DataTemplate NumericDataTemplate { get; set; }
        public DataTemplate StringDataTemplate { get; set; }
        public DataTemplate BoolDataTemplate { get; set; }

        public DataTemplate Vector2DataTemplate { get; set; }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var type = item?.GetType();
            if (type == null) return base.SelectTemplate(null, container);

            if (item is PrimitiveViewModel)
            {
                type = ((PrimitiveViewModel) item).ValueType;
                if (type == typeof(string) && StringDataTemplate != null) return StringDataTemplate;
                if (Numerics.Contains(type) && NumericDataTemplate != null) return NumericDataTemplate;
                if (type == typeof(bool) && BoolDataTemplate != null) return BoolDataTemplate;
            }

            if (item is Vector2ViewModel)
                return Vector2DataTemplate;

            return UnknownDataTemplate ?? base.SelectTemplate(item, container);
        }
    }
}
