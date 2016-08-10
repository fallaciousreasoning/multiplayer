using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels
{
    public class ComplexViewModel : BaseViewModel
    {
        /// <summary>
        /// Indicates whether this complex view model should have complex children
        /// </summary>
        private readonly int recur;

        private string name;

        private object o;

        public object Object
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
            get { return name ?? Object?.GetType().Name; }
            set
            {
                if (Equals(name, value)) return;

                name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BaseViewModel> Properties { get; } = new ObservableCollection<BaseViewModel>();

        public ComplexViewModel(object o, int recur=0)
        {
            this.recur = recur;
            Object = o;
        }

        private void LoadProperties()
        {
            Properties.Clear();

            var type = Object.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                //We're only interested in properties we can read and write to
                if (!property.CanWrite || !property.CanRead) continue;

                if (PrimitiveViewModel.CanConvert(property.PropertyType))
                    Properties.Add(PrimitiveViewModel.Create(Object, property));
                else if (recur > 0)
                {
                    var value = property.GetValue(Object);
                    Properties.Add(new ComplexViewModel(value, recur - 1)
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
                    Properties.Add(PrimitiveViewModel.Create(Object, field));
                else if (recur > 0)
                {
                    var value = field.GetValue(Object);
                    Properties.Add(new ComplexViewModel(value, recur - 1)
                    {
                        Name = field.Name
                    });
                }
            }
        }
    }
}
