using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekickWPF.ProjectWindow;
using RedstoneSidekickWPF.ProjectWindow.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RedstoneSidekickWPF.Commands
{
    public class RemoveItemCommand : ICommand
    {
        private ucCraftingTree _craftingTreeControl;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RemoveItemCommand(ucCraftingTree craftingTreeControl)
        {
            _craftingTreeControl = craftingTreeControl;
        }


        public bool CanExecute(object parameter)
        {
            return _craftingTreeControl.CraftingTreeView.SelectedItem != null;
        }

        public void Execute(object parameter)
        {
            ICraftingTreeItem item = (ICraftingTreeItem) _craftingTreeControl.CraftingTreeView.SelectedItem;

            var treeViewItem = _craftingTreeControl.CraftingTreeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
            treeViewItem.IsSelected = false;

            _craftingTreeControl.Items.Remove(item);
        }
    }
}
