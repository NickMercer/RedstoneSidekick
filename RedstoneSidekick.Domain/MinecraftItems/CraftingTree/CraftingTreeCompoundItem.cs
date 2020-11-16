using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems
{
    public class CraftingTreeCompoundItem : ICraftingTreeCompoundItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string ImagePath { get; set; }
        
        public string MinecraftId { get; set; }
        
        public ItemCategory Category { get; set; }
        
        public int Count { get; set; }
        
        public bool IsChecked { get; set; }
        
        public List<ICraftingTreeItem> Ingredients { get; set; }
    }
}
