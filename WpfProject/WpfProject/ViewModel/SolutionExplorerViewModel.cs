//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
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
                    SaveAsCommand.RaiseCanExecuteChanged();
                    CloseAllTabsCommand.RaiseCanExecuteChanged();
                    CloseCurrentTabCommand.RaiseCanExecuteChanged();
                    AddNewFolderCommand.RaiseCanExecuteChanged();
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
            SaveAsCommand = new BaseCommand(OnSaveAs, CanSaveAs);
            CloseAllTabsCommand = new BaseCommand(OnCloseAllTabs, CanCloseTabs);
            CloseCurrentTabCommand = new BaseCommand(OnCloseCurrentTab, CanCloseTabs);
            AddNewFileCommand = new BaseCommand(OnAddNewFile, CanAddNewFile);
            AddNewFolderCommand = new BaseCommand(OnAddNewFolder, CanContextMenuAddFile);
           // ContextMenuAddFileCommand = new BaseCommand()
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

        void OnCloseCurrentTab() {
            Tabs.Remove(SelectedItem);
        }
        void OnCloseAllTabs() {
            Tabs.Clear();
        }
        bool CanCloseTabs() {
            if(Tabs.Count>0) return true;
            else return false;
        }

        void OnSaveAs1() {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
                string filePath = saveFileDialog1.FileName;
                string fileText = SelectedItem.Text;
                File.WriteAllText(filePath,fileText );
                string fileDirectory = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                ProjectItem newItem = new ProjectItem(CloseTab) {Path=fileDirectory ,Name= fileName, Type=ProjectItemType.File,
                Text=fileText };
                AddItemToItems(newItem,Items[0]);
            }
        }

        void OnSaveAs() {
            AddNewFile(SelectedItem.Text);
        }
        bool CanSaveAs() {
            if(SelectedItem?.Type != ProjectItemType.File) return false;
            else return true;
        }

        void OnAddNewFile() {
            AddNewFile("");
        }
        void OnAddNewFolder() {
            ChoseFolderWindow choseFolderWindow = new ChoseFolderWindow();
            if(choseFolderWindow.ShowDialog() == true) {
                string folderName = choseFolderWindow.FolderName;
                if( folderName != "" && SelectedItem.Type != ProjectItemType.File) {
                    DirectoryInfo di;
                    if(SelectedItem != null )
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

        void OnContextMenuAddFile() {
            if(SelectedItem?.Type !=ProjectItemType.File)
                AddNewFile("");
        }

        bool CanContextMenuAddFile() {
            if(SelectedItem.Type != ProjectItemType.File)
                return true;
            return false;
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
                    Path = fileDirectory, Name = fileName, Type = ProjectItemType.File,Text = fileText
                };
                AddItemToItems(newItem, Items[0]);
            }
        }
        void AddItemToItems(ProjectItem newItem, ProjectItem root) {
            string newItemPerentPath = newItem.Path.Remove(newItem.Path.Length - newItem.Name.Length-1, newItem.Name.Length+1);
            foreach(ProjectItem x in root.Items) {
                if(x.Path == newItemPerentPath && x.Type != ProjectItemType.File) {
                    x.Items.Add(newItem);
                    return;
                }
                AddItemToItems(newItem, x);
            }

        }
    }
}
