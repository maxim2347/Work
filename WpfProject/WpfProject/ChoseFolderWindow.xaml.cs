using System.Windows;

namespace WpfProject {
    public partial class NewFolderWindow : Window {
        public NewFolderWindow() {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}
