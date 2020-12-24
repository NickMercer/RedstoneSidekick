using RedstoneSidekickWPF.ProjectWindow;
using RedstoneSidekickWPF.ProjectWindow.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class AddItemCommand : ICommand
    {
        private ucCraftingTree _craftingTreeControl;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AddItemCommand(ucCraftingTree craftingTreeControl)
        {
            _craftingTreeControl = craftingTreeControl;
        }

        
        public bool CanExecute(object parameter)
        {
            return _craftingTreeControl != null;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
