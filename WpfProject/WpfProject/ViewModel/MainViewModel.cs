//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class MainViewModel : BaseVM {
        ProjectItem selectedItem;
        bool isSelected;

        public BaseCommand SaveCommand { get; }
        public BaseCommand SaveAsCommand { get; }
        public BaseCommand CloseAllTabsCommand { get; }
        public BaseCommand CloseCurrentTabCommand { get; }
        public BaseCommand AddNewFileCommand { get; }
        public BaseCommand AddNewFolderCommand { get; }
        public BaseCommand ContextMenuAddFileCommand { get; }
        public ObservableCollection<ProjectItem> Items { get; }
        public ObservableCollection<ProjectItem> Tabs { get; }
        public ProjectItem SelectedItem { get { return selectedItem; } set { SetProperty(ref selectedItem, value, OnSelectedItemChanged); } }
        public bool IsSelected { get { return isSelected; } set { SetProperty(ref isSelected, value); } }

        public MainViewModel() {
            string startDirectory = @"C:\Work\Work\WpfProject\ProjectA";
            Items = new ObservableCollection<ProjectItem>();
            Tabs = new ObservableCollection<ProjectItem>();
            ProjectItem Root = new ProjectItem(CloseTab) { Name = "ProjectA", Type = ProjectItemType.Project };
            Items.Add(Root);
            GetDirectoryTree(startDirectory, Root);

            SaveCommand = new BaseCommand(Save, CanSave);
            SaveAsCommand = new BaseCommand(SaveAs, CanSaveAs);
            CloseAllTabsCommand = new BaseCommand(CloseAllTabs, CanCloseTabs);
            CloseCurrentTabCommand = new BaseCommand(CloseCurrentTab, CanCloseTabs);
            AddNewFileCommand = new BaseCommand(AddNewFile, CanAddNewFile);
            AddNewFolderCommand = new BaseCommand(AddNewFolder, CanContextMenuAddFile);
        }

        void OnSelectedItemChanged(ProjectItem oldValue, ProjectItem newValue) {
            if(oldValue != null)
                oldValue.TextChanged -= OnTextChanged;
            if(newValue != null)
                newValue.TextChanged += OnTextChanged;
            SaveCommand.RaiseCanExecuteChanged();
            SaveAsCommand.RaiseCanExecuteChanged();
            CloseAllTabsCommand.RaiseCanExecuteChanged();
            CloseCurrentTabCommand.RaiseCanExecuteChanged();
            AddNewFolderCommand.RaiseCanExecuteChanged();
        }

        void GetDirectoryTree(string start, ProjectItem root) {
            string[] dd = Directory.GetDirectories(start);
            foreach(var x in dd) {
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

        void Save() {
            File.WriteAllText(SelectedItem.Path, SelectedItem.Text);
        }
        bool CanSave() {
            if(SelectedItem?.Type != ProjectItemType.File) return false;
            string sourseText = File.ReadAllText(SelectedItem.Path);
            if(SelectedItem.Text != sourseText)
                return true;
            return false;
        }
        void SaveAs() {
            AddNewFile(SelectedItem.Text);
        }
        bool CanSaveAs() {
            return SelectedItem?.Type == ProjectItemType.File;
        }
        void CloseAllTabs() {
            Tabs.Clear();
        }
        void CloseCurrentTab() {
            Tabs.Remove(SelectedItem);
        }
        bool CanCloseTabs() {
            return Tabs.Count > 0;
        }
        void CloseTab(ProjectItem item) {
            Tabs.Remove(item);
        }

        void AddNewFile() {
            AddNewFile("");
        }
        void AddNewFolder() {
            var newFolderVM = new NewFolderViewModel();
            if(ShowNewFolderDialog(newFolderVM) == true) {
                string folderName = newFolderVM.FolderName;
                if(folderName != "" && SelectedItem.Type != ProjectItemType.File) {
                    DirectoryInfo di;
                    if(SelectedItem != null)
                        di = Directory.CreateDirectory(SelectedItem.Path + "\\" + folderName);
                    else
                        di = Directory.CreateDirectory(@"C:\Work\Work\WpfProject\ProjectA\" + folderName);
                    ProjectItem newItem = new ProjectItem(CloseTab) {
                        Path = di.FullName, Name = folderName, Type = ProjectItemType.Folder
                    };
                    AddItemToItems(newItem, Items[0]);
                    MessageBox.Show("Folder created");
                } else
                    MessageBox.Show("Incorrect folder name");

            }
        }
        bool CanAddNewFile() {
            return true;
        }
        void AddNewFile(string text) {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
                string filePath = saveFileDialog1.FileName;
                string fileText = text;
                File.WriteAllText(filePath, fileText);
                string fileDirectory = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                ProjectItem newItem = new ProjectItem(CloseTab) {
                    Path = fileDirectory, Name = fileName, Type = ProjectItemType.File, Text = fileText
                };
                AddItemToItems(newItem, Items[0]);
            }
        }
        void AddItemToItems(ProjectItem newItem, ProjectItem root) {
            string newItemPerentPath = newItem.Path.Remove(newItem.Path.Length - newItem.Name.Length - 1, newItem.Name.Length + 1);
            foreach(ProjectItem x in root.Items) {
                if(x.Path == newItemPerentPath && x.Type != ProjectItemType.File) {
                    x.Items.Add(newItem);
                    return;
                }
                AddItemToItems(newItem, x);
            }

        }

        void OnContextMenuAddFile() {
            if(SelectedItem?.Type != ProjectItemType.File)
                AddNewFile("");
        }
        bool CanContextMenuAddFile() {
            if(SelectedItem.Type != ProjectItemType.File)
                return true;
            return false;
        }

        void OnTextChanged(object sender, EventArgs e) {
            SaveCommand.RaiseCanExecuteChanged();
        }

        protected virtual bool? ShowNewFolderDialog(NewFolderViewModel vm) {
            var newFolderWindow = new NewFolderWindow() { DataContext = vm };
            return newFolderWindow.ShowDialog();
        }
    }
}
