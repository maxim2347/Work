using System.Collections.Generic;
using WpfProject.Common;
using WpfProject.Model;

namespace WpfProject.ViewModel {
    public class SolutionExplorerViewModel : BaseVM {
        public List<ProjectItem> Items { get; }
        
        ProjectItem selectedItem;
        public ProjectItem SelectedItem { get { return selectedItem; } set { SetProperty(ref selectedItem, value); } } 

        public SolutionExplorerViewModel() {
            Items = new List<ProjectItem>();
         
            var projectA = new ProjectItem() {
                Name = "ProjectA",
                Type = ProjectItemType.Project,
            };
            var projectProperties = new ProjectItem() {
                Name = "Properties",
                Type = ProjectItemType.Folder,
            };
            var projectPropertiesFile = new ProjectItem() {
                Name = "Settings.txt",
                Type = ProjectItemType.File,
                Text = "Settings.txt text text",
            };
            projectProperties.Items.Add(projectPropertiesFile);
            projectA.Items.Add(projectProperties);

            var file = new ProjectItem() {
                Name = "File.txt",
                Type = ProjectItemType.File,
                Text = "File.txt text text",
            };
            projectA.Items.Add(file);
            Items.Add(projectA);
        }
    }
}
