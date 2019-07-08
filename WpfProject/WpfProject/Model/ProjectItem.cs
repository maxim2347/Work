using System.Collections.Generic;
using WpfProject.Common;
using System;

namespace WpfProject.Model {
    public enum ProjectItemType { Project, Folder, File }

    public class ProjectItem : BaseVM {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectItemType Type { get; set; }
        public List<ProjectItem> Items { get; }
        public string Text { get; set; }

        Action<ProjectItem> onClosed;
        public ProjectItem(Action<ProjectItem> onClosed) {
            Items = new List<ProjectItem>();
            this.onClosed = onClosed;
            CloseTabCommand = new BaseCommand(OnCloseTabCommandExecute);
        }
        public BaseCommand CloseTabCommand { get; private set; }
        void OnCloseTabCommandExecute() {
            onClosed(this);
        }
    }
}
