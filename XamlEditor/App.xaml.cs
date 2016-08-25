using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;
using XamlEditor.ViewModels.PropertySheets;

namespace XamlEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            PropertySheetManager.Initialize();
            PropertySheetManager.RegisterViewModelForType(typeof(Vector2), typeof(Vector2ViewModel));
        }
    }
}
