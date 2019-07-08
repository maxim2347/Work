using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
        bool isEnabled;
        ProjectItem selectedItem;
        bool isSelected;
        
        public BaseCommand CloseTabCommand { get; }
        public BaseCommand SaveCommand { get; }
        public ObservableCollection<ProjectItem> Items { get; }
        public ObservableCollection<ProjectItem> Tabs { get; }
        public bool IsEnabled { get { return isEnabled; } set { SetProperty(ref isEnabled, value); } }
        public ProjectItem SelectedItem { get { return selectedItem; } set { SetProperty(ref selectedItem, value);  } }
        public bool IsSelected { get { return isSelected; } set {SetProperty(ref isSelected, value); } }
        
        public SolutionExplorerViewModel() {
            string startDirectory = @"C:\Work\Work\WpfProject\ProjectA";
            Items = new ObservableCollection<ProjectItem>();
            Tabs = new ObservableCollection<ProjectItem>();
            ProjectItem Root = new ProjectItem() { Name = "ProjectA", Type = ProjectItemType.Project };
            Root.CloseTabCommand = new BaseCommand(() => CloseTab(Root));
            Items.Add(Root);
            GetDirectoryTree(startDirectory, Root);
            SaveCommand = new BaseCommand(OnSave);
            CloseTabCommand = new BaseCommand(CloseSelectedItem);
        }

        void GetDirectoryTree(string start ,ProjectItem root) {
            string[] dd = Directory.GetDirectories(start);
            foreach(var x in dd) {
                string x1 = x;
                x1.Remove(0,start.Length);
                ProjectItem item = new ProjectItem() {Path=x, Name = x.Remove(0, start.Length+1), Type =ProjectItemType.Folder };
                item.CloseTabCommand = new BaseCommand(() => CloseTab(item));
                root.Items.Add(item);
                GetDirectoryTree(x, item);
            }
            foreach(var y in Directory.GetFiles(start)) {
                ProjectItem file = new ProjectItem() { Path = y,
                    Name = y.Remove(0, start.Length + 1), Type = ProjectItemType.File,Text= File.ReadAllText(y) };
                file.CloseTabCommand = new BaseCommand(() => CloseTab(file));
                root.Items.Add(file);
            }
        }

        void OnSave() {
            File.WriteAllText(SelectedItem.Path, SelectedItem.Text);
        }

        void CloseSelectedItem() {
            CloseTab(SelectedItem);
        }
        void CloseTab(ProjectItem item) {
            Tabs.Remove(item);
        }

    }
}
