using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using XamlEditor.ViewModels.PropertySheets;

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

        public static IEnumerable<object> GetAllAttributes(this Type type, FieldInfo fieldInfo)
        {
            var attributes = new List<object>();
            attributes.AddRange(fieldInfo.GetCustomAttributes(true));

            var interfaces = type.GetInterfaces();

            foreach (var i in interfaces)
            {
                var iPropertyInfo = i
                    .GetProperties()
                    .FirstOrDefault(p => p.PropertyType == fieldInfo.FieldType && p.Name == fieldInfo.Name);
                if (iPropertyInfo == null) continue;
                attributes.AddRange(iPropertyInfo.GetCustomAttributes(true));
            }

            return attributes;
        }

        public static IEnumerable<IAccessor> GetAccessors(this Type type)
        {
            var result = new List<IAccessor>();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                //We're only interested in fields we can read and write to
                if (!property.CanRead || !property.CanWrite || ShouldIgnore(type.GetAllAttributes(property)))
                    continue;

                //If we shouldn't ever add a field for this type, continue
                if (ShouldIgnore(property.PropertyType.GetAllAttributes())) continue;


                result.Add(new PropertyAccessor(property));
            }

            var fields = type.GetFields();

            foreach (var field in fields)
            {
                //We're only interested in fields we can read and write to
                if (!field.IsPublic || field.IsLiteral || field.IsStatic || field.IsInitOnly || ShouldIgnore(type.GetAllAttributes(field)))
                    continue;

                //If we shouldn't ever add a field for this type, continue
                if (ShouldIgnore(field.FieldType.GetAllAttributes())) continue;
                

                result.Add(new FieldAccessor(field));
            }

            return result;
        }

        private static bool ShouldIgnore(IEnumerable<object> attributes)
        {
            foreach (var attribute in attributes)
                if (attribute is EditorIgnoreAttribute)
                    return true;
            return false;
        }
    }
}
