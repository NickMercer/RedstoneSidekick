using RedstoneSidekickWPF.ProjectWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class AddStructureToProjectCommand : ICommand
    {
        private readonly ProjectWindowVM _vm;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AddStructureToProjectCommand(ProjectWindowVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return _vm.Project.CraftingTree != null && _vm.Project.CraftingTree.Items.Any();
        }

        public void Execute(object parameter)
        {
            _vm.AddStructureToProject();
        }
    }
}
