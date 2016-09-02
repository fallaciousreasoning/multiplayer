using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace XamlEditor.ViewModels.PropertySheets
{
    public class Vector2ViewModel : BaseViewModel, IValueViewModel
    {
        private string name;
        private Vector2 value;
        private object o;
        private IAccessor accessor;

        public void Reload()
        {
            Value = accessor.GetValue(Object);
        }

        public string Name
        {
            get { return name ?? Accessor?.Name; }
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public object Value
        {
            get { return value; }
            set
            {
                if (Equals(value, this.value)) return;
                this.value = (Vector2)value;

                Set();

                OnPropertyChanged();
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
            }
        }

        public float X
        {
            get { return value.X; }
            set
            {
                if (value.Equals(this.value.X)) return;

                this.value.X = value;

                Set();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        public float Y
        {
            get { return value.Y; }
            set
            {
                if (value.Equals(this.value.Y)) return;
                this.value.Y = value;

                Set();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
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
                OnPropertyChanged(nameof(Value));
            }
        }

        public IAccessor Accessor
        {
            get { return accessor; }
            set
            {
                if (Equals(value, accessor)) return;

                if(value.ValueType != typeof(Vector2)) 
                    throw new ArgumentException("Only Vector2's are supported");

                accessor = value;

                Get();

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
            }
        }

        public ObservableCollection<IValueViewModel> Children { get; } = null;

        private void Set()
        {
            if (Accessor == null || Object == null) return;

            Accessor.SetValue(Object, value);
        }

        private void Get()
        {
            if (Accessor == null || Object == null) return;

            this.value = (Vector2)Accessor.GetValue(Object);
        }
    }
}
