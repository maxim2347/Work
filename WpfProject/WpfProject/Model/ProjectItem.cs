using System.Collections.Generic;
using WpfProject.Common;
using System;
using System.Collections.ObjectModel;

namespace WpfProject.Model {
    public enum ProjectItemType { Project, Folder, File }

    public class ProjectItem : BaseVM {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectItemType Type { get; set; }
        public ObservableCollection<ProjectItem> Items { get; }
        public string text;
        public string Text { get { return text; }
            set {
                if(text == value) return;
                text = value;
                RiseTextChange();
            }
        }

        Action<ProjectItem> onClosed;
        public ProjectItem(Action<ProjectItem> onClosed) {
            Items = new ObservableCollection<ProjectItem>();
            this.onClosed = onClosed;
            CloseTabCommand = new BaseCommand(OnCloseTabCommandExecute);
        }
        public BaseCommand CloseTabCommand { get; private set; }
        void OnCloseTabCommandExecute() {
            onClosed(this);
        }

        public event EventHandler TextChanged;
        public void RiseTextChange() {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}

