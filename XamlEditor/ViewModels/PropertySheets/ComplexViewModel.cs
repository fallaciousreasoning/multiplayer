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
        private string name;

        private object value;
        private object o;
        private PropertyInfo propertyInfo;

        public object Value
        {
            get { return value; }
            set
            {
                if (Equals(value, this.value)) return;
                this.value = value;

                LoadProperties();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Name
        {
            get { return name ?? PropertyInfo?.Name ?? Value?.GetType().Name; }
            set
            {
                if (Equals(name, value)) return;

                name = value;
                OnPropertyChanged();
            }
        }

        public object Object
        {
            get { return o; }
            set
            {
                if (Equals(value, o)) return;
                o = value;
                OnPropertyChanged();
            }
        }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
            set
            {
                if (Equals(value, propertyInfo)) return;
                propertyInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IPropertyViewModel> Children { get; } = new ObservableCollection<IPropertyViewModel>();

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

                //If we shouldn't ever add a property for this type, continue
                if (ShouldIgnore(property.PropertyType.GetAllAttributes())) continue;

                var viewModel = PropertySheetManager.GetViewModelFor(property.PropertyType);
                if (viewModel == null) continue;

                viewModel.PropertyInfo = property;
                viewModel.Object = Value;

                Children.Add(viewModel);
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
