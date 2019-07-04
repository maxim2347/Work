using System.Collections.Generic;
using System.IO;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
        public List<ProjectItem> Items { get; }
     
        ProjectItem selectedItem;
        public ProjectItem SelectedItem { get { return selectedItem; } set { SetProperty(ref selectedItem, value); } }

        void GetDirectoryTree(string start ,ProjectItem root) {
            string[] dd = Directory.GetDirectories(start);
            foreach(var x in dd) {
                string x1 = x;
                x1.Remove(0,start.Length);
                ProjectItem item = new ProjectItem() { Name = x.Remove(0, start.Length+1), Type =ProjectItemType.Folder };
                root.Items.Add(item);
                GetDirectoryTree(x, item);
            }
            foreach(var y in Directory.GetFiles(start)) {
                ProjectItem file = new ProjectItem() { Name = y.Remove(0, start.Length + 1), Type=ProjectItemType.File };
                root.Items.Add(file);
            }
        }

        public SolutionExplorerViewModel() {
            string startDirectory = @"C:\Work\Work\WpfProject\ProjectA";
            Items= new List<ProjectItem>();
            ProjectItem Root = new ProjectItem() { Name = "ProjectA", Type = ProjectItemType.Project };
            Items.Add(Root);
           
            GetDirectoryTree(startDirectory,Root);
            
        }
    }
}
