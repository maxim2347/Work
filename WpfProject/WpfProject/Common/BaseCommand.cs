using System;
using System.Windows;
using System.Windows.Input;

namespace WpfProject.Common {
    public class BaseCommand : ICommand {
        public event EventHandler CanExecuteChanged; 

        readonly Action action1;
        readonly Action<object> action2;
        readonly Func<object,bool> canExecuteParameter;
        readonly Func<bool> canExecute;

        public BaseCommand(Action action ,Func<bool> canExecute) {
            this.action1 = action;
            this.canExecute = canExecute;
        }
        public BaseCommand(Action<object> action, Func<object, bool> canExecuteParametr) {
            this.action2 = action;
            this.canExecuteParameter = canExecuteParametr;
        }
        void ICommand.Execute(object parameter) {
            action1?.Invoke();
            action2?.Invoke(parameter);
        }
        bool ICommand.CanExecute(object parameter) {
            return this.canExecute?.Invoke()
                ?? this.canExecuteParameter?.Invoke(parameter)
                ?? true;
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
    public class VM {
        BaseCommand MyCommand { get; set; }
        public VM() {
            MyCommand = new BaseCommand(Execute, CanExecute);
        }
        void Execute(object parameter) {
            MessageBox.Show("!");
        }
        bool CanExecute(object parameter) {
            return true;
        }
    }

}

