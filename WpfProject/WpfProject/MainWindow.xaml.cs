using System.Windows;
using WpfProject.Model;
using WpfProject.ViewModel;

namespace WpfProject {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var dc = DataContext as SolutionExplorerViewModel;
            dc.SelectedItem = e.NewValue as ProjectItem;
        }
    }
}
