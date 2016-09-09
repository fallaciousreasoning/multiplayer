using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MultiPlayer.Core;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.ViewModels
{
    public class ComponentPropertyViewModel : BaseViewModel, IValueViewModel, IDropTarget
    {
        private string name;

        private object value;
        private object o;
        private IAccessor accessor;

        public object Value
        {
            get { return accessor.GetValue(Object); }
            set
            {
                if (Equals(value, Value)) return;

                accessor.SetValue(Object, value);

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(FriendlyValue));
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

        public string FriendlyValue => Value?.GetType().Name ?? "None";

        public ObservableCollection<IValueViewModel> Children { get; } = new ObservableCollection<IValueViewModel>();
        

        public void Reload()
        {
            Value = accessor.GetValue(Object);
            OnPropertyChanged(nameof(Value));
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is EntityHierarchyViewModel)) return;
            var entity = ((EntityHierarchyViewModel) dropInfo.Data).Entity;

            if (!entity.HasComponent(accessor.ValueType)) return;
            
            dropInfo.NotHandled = false;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is EntityHierarchyViewModel)) return;

            var entity = ((EntityHierarchyViewModel) dropInfo.Data).Entity;
            Value = entity.Get(accessor.ValueType);
        }
    }
}
