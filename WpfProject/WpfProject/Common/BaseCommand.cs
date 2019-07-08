using System;
using System.Windows.Input;

namespace WpfProject.Common {
    public class BaseCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        readonly Action action;
        public BaseCommand(Action action) {
            this.action = action;
        }
        public void Execute(object parameter) {
            action();
        }
        public bool CanExecute(object parameter) {
            return true;
        }
    }
}
