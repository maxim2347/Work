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
        }
        void OnSaveButtonClick(object sender, RoutedEventArgs e) {
            var dc = DataContext as SolutionExplorerViewModel;
            File.WriteAllText(dc.SelectedItem.Path, dc.SelectedItem.Text);
        }
        void OnTextBoxTextChanged(object sender, TextChangedEventArgs e) {
            var dc = DataContext as SolutionExplorerViewModel;
            try {
                string sourseText = File.ReadAllText(dc.SelectedItem.Path);
                if(dc.SelectedItem.Text != sourseText)
                    dc.IsEnabled = true;
                else dc.IsEnabled = false;
            } catch { };

        }

        TreeViewItem TryGetClickedItem(TreeView treeView, MouseButtonEventArgs e) {
            var hit = e.OriginalSource as DependencyObject;
            while(hit != null && !(hit is TreeViewItem))
                hit = VisualTreeHelper.GetParent(hit);
            return hit as TreeViewItem;
        }
    }
}
