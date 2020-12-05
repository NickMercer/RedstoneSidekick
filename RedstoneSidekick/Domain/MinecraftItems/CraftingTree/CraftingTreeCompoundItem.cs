using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeCompoundItem : ICraftingTreeCompoundItem
    {
        public MinecraftItem Item { get; set; }

        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public bool IsChecked { get; set; }
        
        public List<ICraftingTreeItem> Ingredients { get; set; }
    }
}
