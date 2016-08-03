using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlEditor.ViewModels
{
    public class ScriptViewModel : BaseViewModel
    {
        private object script;

        public object Script
        {
            get { return script; }
            set
            {
                if (Equals(value, script)) return;
                script = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));

                Properties.Clear();
                LoadProperties();
            }
        }

        public string Name => Script.GetType().Name;

        public ObservableCollection<PrimitiveViewModel> Properties { get; private set; } = new ObservableCollection<PrimitiveViewModel>();

        private void LoadProperties()
        {
            var properties = script.GetType().GetProperties();
            foreach (var property in properties)
            {
                //We're only interested in properties we can read and write to
                if (!property.CanWrite || !property.CanRead) continue;

                try
                {
                    //Create the view model and add it to our collection
                    var viewModel = new PrimitiveViewModel()
                    {
                        Object = Script,
                        PropertyInfo = property
                    };
                    Properties.Add(viewModel);
                }
                catch { }
            }
        }
    }
}
