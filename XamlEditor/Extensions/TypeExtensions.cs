using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<object> GetAllAttributes(this Type type)
        {
            var interfaces = type.GetInterfaces();

            var attributes = new List<object>();
            attributes.AddRange(type.GetCustomAttributes(true));

            interfaces.Foreach(i => attributes.AddRange(i.GetCustomAttributes(true)));

            return attributes;
        }

        public static IEnumerable<object> GetAllAttributes(this Type type, PropertyInfo propertyInfo)
        {
            var attributes = new List<object>();
            attributes.AddRange(propertyInfo.GetCustomAttributes(true));

            var interfaces = type.GetInterfaces();

            foreach (var i in interfaces)
            {
                var iPropertyInfo = i
                    .GetProperties()
                    .FirstOrDefault(p => p.PropertyType == propertyInfo.PropertyType && p.Name == propertyInfo.Name);
                if (iPropertyInfo == null) continue;
                attributes.AddRange(iPropertyInfo.GetCustomAttributes(true));
            }

            return attributes;
        }
    }
}
