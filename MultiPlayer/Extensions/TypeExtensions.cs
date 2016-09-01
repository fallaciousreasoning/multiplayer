using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Extensions
{
    public static class TypeExtensions
    {

        public static List<object> GetAllCustomAttributes(this Type type, List<object> addTo=null)
        {
            var interfaces = type.GetInterfaces();
            if (addTo == null) addTo = new List<object>();

            addTo.AddRange(type.GetCustomAttributes(true));

            foreach (var i in interfaces)
                i.GetAllCustomAttributes(addTo);

            return addTo;
        }

        public static List<CustomAttributeData> GetAllCustomAttributesData(this Type type, List<CustomAttributeData> addTo = null)
        {
            var interfaces = type.GetInterfaces();

            if (addTo == null) addTo = new List<CustomAttributeData>();

            addTo.AddRange(type.GetCustomAttributesData());

            foreach (var i in interfaces)
                i.GetAllCustomAttributesData(addTo);

            return addTo;
        }

        public static IEnumerable<Type> ComposingTypes(this Type type)
        {
            return
                type.GetFields()
                    .Where(field => !(field.IsInitOnly || field.IsLiteral || field.IsStatic || !field.IsPublic)).Select(field => field.FieldType);
        }
    }
}
