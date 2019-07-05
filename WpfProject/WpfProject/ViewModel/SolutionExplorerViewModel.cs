using System.Collections.Generic;
using System.IO;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
        bool isEnabled;
        ProjectItem selectedItem;
        bool isSelected;

        public List<ProjectItem> Items { get; }
        public bool IsEnabled { get { return isEnabled; } set { SetProperty(ref isEnabled, value); } }
        public ProjectItem SelectedItem { get { return selectedItem; } set { SetProperty(ref selectedItem, value);  } }
        public bool IsSelected { get { return isSelected; } set {SetProperty(ref isSelected, value); } }

        public SolutionExplorerViewModel() {
            string startDirectory = @"C:\Work\Work\WpfProject\ProjectA";
            Items = new List<ProjectItem>();
            ProjectItem Root = new ProjectItem() { Name = "ProjectA", Type = ProjectItemType.Project };
            Items.Add(Root);

            GetDirectoryTree(startDirectory, Root);
        }

        void GetDirectoryTree(string start ,ProjectItem root) {
            string[] dd = Directory.GetDirectories(start);
            foreach(var x in dd) {
                string x1 = x;
                x1.Remove(0,start.Length);
                ProjectItem item = new ProjectItem() {Path=x, Name = x.Remove(0, start.Length+1), Type =ProjectItemType.Folder };
                root.Items.Add(item);
                GetDirectoryTree(x, item);
            }
            foreach(var y in Directory.GetFiles(start)) {
                ProjectItem file = new ProjectItem() { Path = y,
                    Name = y.Remove(0, start.Length + 1), Type = ProjectItemType.File,Text= File.ReadAllText(y) };
                root.Items.Add(file);
            }
        }
    }
}
