using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XamlEditor.Annotations;

namespace XamlEditor.ViewModels.PropertySheets
{
    public static class PropertySheetManager
    {
        private static readonly Dictionary<Type, Type> ViewModelMap = new Dictionary<Type, Type>();

        public static void RegisterViewModelForType(Type type, Type viewModelType)
        {
            if (!typeof(IPropertyViewModel).IsAssignableFrom(viewModelType))
                throw new ArgumentException($"{viewModelType.FullName} does not extend IPropertyViewModel");

            var constructor = viewModelType.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ArgumentException($"{viewModelType.FullName} does not contain a default constructor");

            ViewModelMap.Add(type, viewModelType);
        }

        public static IPropertyViewModel GetViewModelFor([NotNull] Type type)
        {
            while (!ViewModelMap.ContainsKey(type))
            {
                if (type.BaseType != typeof(object) && type.BaseType != null) type = type.BaseType;
                else return null;
            }

            var viewModelType = ViewModelMap[type];
            return (IPropertyViewModel) Activator.CreateInstance(viewModelType);
        }

        public static void Initialize()
        {
            RegisterViewModelForType(typeof(double), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(float), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(int), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(long), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(short), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(byte), typeof(PrimitiveViewModel));

            RegisterViewModelForType(typeof(char), typeof(PrimitiveViewModel));
            RegisterViewModelForType(typeof(string), typeof(PrimitiveViewModel));

            RegisterViewModelForType(typeof(bool), typeof(PrimitiveViewModel));

            RegisterViewModelForType(typeof(Enum), typeof(EnumViewModel));
        }
    }
}
