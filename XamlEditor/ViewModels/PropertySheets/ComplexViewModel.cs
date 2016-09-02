using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Annotations;
using XamlEditor.Extensions;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.ViewModels
{
    public class ComplexViewModel : BaseViewModel, IValueViewModel
    {
        private string name;

        private object value;
        private object o;
        private IAccessor accessor;

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
            get { return name ?? Accessor?.Name ?? Value?.GetType().Name; }
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

        public IAccessor Accessor
        {
            get { return accessor; }
            set
            {
                if (Equals(value, accessor)) return;
                accessor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IValueViewModel> Children { get; } = new ObservableCollection<IValueViewModel>();

        public void Reload()
        {
            Children.Foreach(c => c.Reload());
        }

        private void LoadProperties()
        {
            Children.Clear();

            var type = Value.GetType();
            var accessors = type.GetAccessors();

            accessors.Foreach(a =>
            {
                var viewModel = PropertySheetManager.GetViewModelFor(a.ValueType);

                if (viewModel == null) return;
                viewModel.Accessor = a;
                viewModel.Object = Value;

                Children.Add(viewModel);
            });
        }
    }
}
