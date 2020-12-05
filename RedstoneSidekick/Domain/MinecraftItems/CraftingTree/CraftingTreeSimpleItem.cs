using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeSimpleItem : ICraftingTreeItem
    {
        public MinecraftItem Item { get; set; }
        
        public int RequiredAmount { get; set; }
        
        public int CurrentAmount { get; set; }
        
        public bool IsChecked { get; set; }

        public CraftingTreeSimpleItem(MinecraftItem item, int requiredAmount, int currentAmount = 0)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            CurrentAmount = currentAmount;
        }
    }
}
