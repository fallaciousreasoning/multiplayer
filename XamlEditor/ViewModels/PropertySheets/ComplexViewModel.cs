using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using XamlEditor.Extensions;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.ViewModels
{
    public class ComplexViewModel : BaseViewModel, IPropertyViewModel
    {
        /// <summary>
        /// Indicates whether this complex view model should have complex children
        /// </summary>
        private readonly int recur;

        private string name;

        private object o;

        public object Value
        {
            get { return o; }
            set
            {
                if (Equals(value, o)) return;
                o = value;

                LoadProperties();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Name
        {
            get { return name ?? Value?.GetType().Name; }
            set
            {
                if (Equals(name, value)) return;

                name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IPropertyViewModel> Children { get; } = new ObservableCollection<IPropertyViewModel>();

        public ComplexViewModel(object o, int recur=0)
        {
            this.recur = recur;
            Value = o;
        }

        private void LoadProperties()
        {
            Children.Clear();

            var type = Value.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                //We're only interested in properties we can read and write to
                if (!property.CanWrite || !property.CanRead || ShouldIgnore(type.GetAllAttributes(property)))
                    continue;

                if (PrimitiveViewModel.CanConvert(property.PropertyType))
                    Children.Add(PrimitiveViewModel.Create(Value, property));
                else if (recur > 0)
                {
                    var value = property.GetValue(Value);
                    Children.Add(new ComplexViewModel(value, recur - 1)
                    {
                        Name = property.Name
                    });
                }
            }

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                //We're only interested in properties we can read and write to
                if (!field.IsPublic || field.IsStatic || field.IsInitOnly || field.IsLiteral) continue;
                
                if (PrimitiveViewModel.CanConvert(field.FieldType))
                    Children.Add(PrimitiveViewModel.Create(Value, field));
                else if (recur > 0 && !ShouldIgnore(field.GetCustomAttributes(true)))
                {
                    var value = field.GetValue(Value);
                    Children.Add(new ComplexViewModel(value, recur - 1)
                    {
                        Name = field.Name
                    });
                }
            }
        }

        private bool ShouldIgnore(IEnumerable<object> attributes)
        {
            foreach (var attribute in attributes)
                if (attribute is EditorIgnoreAttribute)
                    return true;
            return false;
        }
    }
}
