using RedstoneSidekickWPF.ProjectWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class SaveProjectCommand : ICommand
    {
        private readonly ProjectWindowVM _vm;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SaveProjectCommand(ProjectWindowVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return _vm.CraftingTree != null && _vm.CraftingTree.Items.Any();
        }

        public void Execute(object parameter)
        {
            _vm.SaveProject();
        }
    }
}
