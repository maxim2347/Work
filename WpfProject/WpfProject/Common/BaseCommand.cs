using System;
using System.Windows.Input;

namespace WpfProject.Common {
    public class BaseCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        readonly Action action1;
        readonly Action<object> action2;
        public BaseCommand(Action action) {
            this.action1 = action;
        }
        public BaseCommand(Action<object> action) {
            this.action2 = action;
        }
        public void Execute(object parameter) {
            action1?.Invoke();
            action2?.Invoke(parameter);
        }
        public bool CanExecute(object parameter) {
            return true;
        }
    }
}
