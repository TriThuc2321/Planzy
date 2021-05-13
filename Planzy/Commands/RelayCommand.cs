using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Planzy.Commands
{
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Action executeNoObject;
        private Predicate<Object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
            //this.canExecute = canExecute;
        }
        public RelayCommand(Action work)
        {
            this.executeNoObject = work;
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (execute == null)
                this.executeNoObject();
            else this.execute(parameter);

        }
    }
}
