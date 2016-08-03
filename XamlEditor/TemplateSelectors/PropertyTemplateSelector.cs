using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XamlEditor.ViewModels;

namespace XamlEditor.TemplateSelectors
{
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        private readonly Type[] numerics = {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(float),
            typeof(double)
        };

        public DataTemplate ArrayDataTemplate { get; set; }

        public DataTemplate NumericDataTemplate { get; set; }
        public DataTemplate StringDataTemplate { get; set; }
        public DataTemplate BoolDataTemplate { get; set; }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var type = (item as PrimitiveViewModel)?.PropertyInfo?.PropertyType ?? item?.GetType();

            if (type == typeof(string) && StringDataTemplate != null) return StringDataTemplate;
            if (numerics.Contains(type) && NumericDataTemplate != null) return NumericDataTemplate;
            if (type == typeof(bool) && BoolDataTemplate != null) return BoolDataTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
