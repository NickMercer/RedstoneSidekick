using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NatickCommon.WPF;

namespace RedstoneSidekick.WPF.ProjectWindow
{
    public class ProjectWindowVM : ViewModelBase
    {
        private string _projectName = "New Project";
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        public ProjectWindowVM()
        {
            //ItemDataUpdateHandler.UpdateMinecraftItemsDatabase();
        }
    }
}
