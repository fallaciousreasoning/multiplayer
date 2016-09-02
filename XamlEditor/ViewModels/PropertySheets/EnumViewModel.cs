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
    public class EnumViewModel : BaseViewModel, IValueViewModel
    {
        private object value;
        private object o;
        private IAccessor accessor;
        public string Name => Accessor?.Name ?? Value?.GetType().Name;

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

        public IAccessor Accessor
        {
            get { return accessor; }
            set
            {
                if (Equals(value, accessor)) return;
                if (!value.ValueType.IsEnum)
                    throw new ArgumentException("Only enums are supported!");

                accessor = value;

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
            Values.AddAll(Enum.GetValues(Accessor.ValueType));
        }

        private void Get()
        {
            if (Object == null || Accessor == null) return;
            Value = Accessor.GetValue(Object);
        }

        private void Set()
        {
            if (Object == null || Accessor == null) return;

            Accessor.SetValue(Object, Value);
        }

        public ObservableCollection<IValueViewModel> Children { get; } = null;
    }
}
