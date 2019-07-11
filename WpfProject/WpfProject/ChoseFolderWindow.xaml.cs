using System.Windows;

namespace WpfProject {
    public partial class ChoseFolderWindow : Window {
        public ChoseFolderWindow() {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}
