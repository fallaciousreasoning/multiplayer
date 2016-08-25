using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XamlEditor.Extensions;

namespace XamlEditor.ViewModels.PropertySheets
{
    public class EnumViewModel : BaseViewModel, IPropertyViewModel
    {
        private object value;
        private object o;
        private PropertyInfo propertyInfo;
        public string Name => PropertyInfo?.Name ?? Value?.GetType().Name;

        public ObservableCollection<object> Values { get; } = new ObservableCollection<object>();

        public object Value
        {
            get { return value; }
            set
            {
                if (Equals(value, this.value)) return;
                this.value = value;

                Set();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        public object Object
        {
            get { return o; }
            set
            {
                if (Equals(value, o)) return;
                o = value;

                Get();

                OnPropertyChanged();
            }
        }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
            set
            {
                if (Equals(value, propertyInfo)) return;
                if (!value.PropertyType.IsEnum)
                    throw new ArgumentException("Only enums are supported!");

                propertyInfo = value;

                LoadValues();
                Get();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Values));
            }
        }

        private void LoadValues()
        {
            Values.Clear();
            Values.AddAll(Enum.GetValues(PropertyInfo.PropertyType));
        }

        private void Get()
        {
            if (Object == null || PropertyInfo == null) return;
            Value = PropertyInfo.GetValue(Object);
        }

        private void Set()
        {
            if (Object == null || PropertyInfo == null) return;

            PropertyInfo.SetValue(Object, Value);
        }

        public ObservableCollection<IPropertyViewModel> Children { get; } = null;
    }
}
