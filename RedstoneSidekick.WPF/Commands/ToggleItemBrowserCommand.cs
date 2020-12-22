using RedstoneSidekickWPF.ProjectWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class ToggleItemBrowserCommand : ICommand
    {
        private ProjectWindowVM _projectWindowVM;

        public ToggleItemBrowserCommand(ProjectWindowVM projectWindowVM)
        {
            _projectWindowVM = projectWindowVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _projectWindowVM != null;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
