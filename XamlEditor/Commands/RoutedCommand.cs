using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XamlEditor.Commands
{
    public class RoutedCommand : ICommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;

        public RoutedCommand(Action execute)
            : this(o=> execute(), null)
        {
        }

        public RoutedCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RoutedCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
