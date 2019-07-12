using NUnit.Framework;
using System.Linq;
using WpfProject.Model;
using WpfProject.ViewModel;

namespace WpfProject.Tests {
    [TestFixture]
    public class MainViewModelTests {
        [Test]
        public void SaveCommandTest() {
            var vm = new MainViewModel();
            int saveCommandCanExecuteChangedCount = 0;
            vm.SaveCommand.CanExecuteChanged += (s, e) => saveCommandCanExecuteChangedCount++;

            vm.SelectedItem = vm.Items[0];
            Assert.AreEqual(false, vm.SaveCommand.CanExecute(null));
            Assert.AreEqual(1, saveCommandCanExecuteChangedCount);

            vm.SelectedItem = vm.Items[0].Items.First(x => x.Type == ProjectItemType.File);
            Assert.AreEqual(false, vm.SaveCommand.CanExecute(null));
            Assert.AreEqual(2, saveCommandCanExecuteChangedCount);

            vm.SelectedItem.Text += "1";
            Assert.AreEqual(true, vm.SaveCommand.CanExecute(null));
            Assert.AreEqual(3, saveCommandCanExecuteChangedCount);
        }
    }
}
