using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfProject.Model;
using WpfProject.ViewModel;

namespace WpfProject {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
      
        void OnTreeViewPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var dc = DataContext as SolutionExplorerViewModel;

            TreeViewItem clickedItem = TryGetClickedItem(tree, e);
            if(clickedItem == null)
                return;

            dc.SelectedItem = (ProjectItem)clickedItem.DataContext;
            if(dc.SelectedItem?.Type == ProjectItemType.File) {
                bool isAlradyTab = false;
                foreach(var x in dc.Tabs) {
                    if(x.Name == dc.SelectedItem.Name)
                        isAlradyTab = true;
                }
                if(!isAlradyTab)dc.Tabs.Add(dc.SelectedItem);
            }
        }

        TreeViewItem TryGetClickedItem(TreeView treeView, MouseButtonEventArgs e) {
            var hit = e.OriginalSource as DependencyObject;
            while(hit != null && !(hit is TreeViewItem))
                hit = VisualTreeHelper.GetParent(hit);
            return hit as TreeViewItem;
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var dc = DataContext as SolutionExplorerViewModel;
            dc.SelectedItem = e.NewValue as ProjectItem;
        }
    }
}
