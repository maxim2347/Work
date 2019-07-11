using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfProject.Common {
    public class BaseVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
            return SetPropertyCore(ref storage, value, null, propertyName);
        }
        protected bool SetProperty<T>(ref T storage, T value, Action<T, T> callback, [CallerMemberName] string propertyName = null) {
            return SetPropertyCore(ref storage, value, callback, propertyName);
        }
        bool SetPropertyCore<T>(ref T storage, T value, Action<T, T> callback, string propertyName) {
            if(object.Equals(storage, value)) return false;
            var oldValue = storage;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            callback?.Invoke(oldValue, value);
            return true;
        }
    }
}
