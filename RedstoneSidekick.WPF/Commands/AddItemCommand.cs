using Natick.Utilities.ViewModels;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Logic.CraftingTree;
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
            var itemToAdd = _craftingTreeControl.LV_MinecraftItems.SelectedItem as IMinecraftItem;
            if (itemToAdd != null && _craftingTreeControl.Items.FirstOrDefault(x => x.IsRootItem && x.Item.Id == itemToAdd.Id) == null)
            {
                var craftingTree = _craftingTreeControl.Items;
                craftingTree.Add(CraftingTreeItemBuilder.CreateCraftingTreeItem(itemToAdd.Id, 1));
            }
        }
    }
}
