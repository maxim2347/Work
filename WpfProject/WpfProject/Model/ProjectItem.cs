using System.Collections.Generic;
using WpfProject.Common;

namespace WpfProject.Model {
    public enum ProjectItemType { Project, Folder, File }
    public class ProjectItem : BaseVM {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectItemType Type { get; set; }
        public List<ProjectItem> Items { get; }
        public string Text { get; set; }
        public ProjectItem() {
            Items = new List<ProjectItem>();
        }
    }
}
