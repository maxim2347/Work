using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfProject.Common;

namespace WpfProject.ViewModel {
    public class NewFolderViewModel : BaseVM {
        string folderName;
        public string FolderName { get { return folderName; } set { SetProperty(ref folderName, value); } }
    }
}
