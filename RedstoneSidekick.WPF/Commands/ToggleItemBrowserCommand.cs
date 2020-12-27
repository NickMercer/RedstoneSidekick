using RedstoneSidekickWPF.ProjectWindow;
using RedstoneSidekickWPF.ProjectWindow.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class ToggleItemBrowserCommand : ICommand
    {
        private ucCraftingTree _craftingTreeControl;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ToggleItemBrowserCommand(ucCraftingTree craftingTreeControl)
        {
            _craftingTreeControl = craftingTreeControl;
        }


        public bool CanExecute(object parameter)
        {
            return _craftingTreeControl != null;
        }

        public void Execute(object parameter)
        {
            var controlVis = _craftingTreeControl.AddItemsPanel.Visibility;
            controlVis = controlVis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            _craftingTreeControl.AddItemsPanel.Visibility = controlVis;
        }
    }
}
