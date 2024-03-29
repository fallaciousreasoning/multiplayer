﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor.ViewModels
{
    public class ArrayViewModel : BaseViewModel, IValueViewModel
    {
        public void Reload()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public object Object { get; set; }
        public IAccessor Accessor { get; set; }
        public ObservableCollection<IValueViewModel> Children { get; }
    }
}
