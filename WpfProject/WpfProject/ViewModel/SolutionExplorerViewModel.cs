using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
        ProjectItem selectedItem;
        bool isSelected;

        public BaseCommand SaveCommand { get; }
        public ObservableCollection<ProjectItem> Items { get; }
        public ObservableCollection<ProjectItem> Tabs { get; }
        public ProjectItem SelectedItem {
            get { return selectedItem; }
            set {
                var oldValue = selectedItem;
                if(SetProperty(ref selectedItem, value)) {
                    if(oldValue != null)
                        oldValue.TextChanged -= OnTextChanged;
                    if(selectedItem != null)
                        selectedItem.TextChanged += OnTextChanged;
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public bool IsSelected { get { return isSelected; } set { SetProperty(ref isSelected, value); } }

        public SolutionExplorerViewModel() {
            string startDirectory = @"C:\Work\Work\WpfProject\ProjectA";
            Items = new ObservableCollection<ProjectItem>();
            Tabs = new ObservableCollection<ProjectItem>();
            ProjectItem Root = new ProjectItem(CloseTab) { Name = "ProjectA", Type = ProjectItemType.Project };
            Items.Add(Root);
            GetDirectoryTree(startDirectory, Root);
            SaveCommand = new BaseCommand(OnSave, CanSave);
        }

        void GetDirectoryTree(string start, ProjectItem root) {
            string[] dd = Directory.GetDirectories(start);
            foreach(var x in dd) {
                string x1 = x;
                x1.Remove(0, start.Length);
                ProjectItem item = new ProjectItem(CloseTab) { Path = x, Name = x.Remove(0, start.Length + 1), Type = ProjectItemType.Folder };
                root.Items.Add(item);
                GetDirectoryTree(x, item);
            }
            foreach(var y in Directory.GetFiles(start)) {
                ProjectItem file = new ProjectItem(CloseTab) {
                    Path = y,
                    Name = y.Remove(0, start.Length + 1), Type = ProjectItemType.File, Text = File.ReadAllText(y)
                };
                root.Items.Add(file);
            }
        }

        void OnSave() {
            File.WriteAllText(SelectedItem.Path, SelectedItem.Text);
        }
        bool CanSave() {
            if(SelectedItem?.Type != ProjectItemType.File) return false;
            string sourseText = File.ReadAllText(SelectedItem.Path);
            if(SelectedItem.Text != sourseText)
                return true;
            return false;
        }

        void OnTextChanged(object sender, EventArgs e) {
            SaveCommand.RaiseCanExecuteChanged();
        }

        void CloseTab(ProjectItem item) {
            Tabs.Remove(item);
        }

    }
}
